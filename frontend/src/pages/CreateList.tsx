import { useState, useCallback, useEffect, useRef, useMemo } from 'react';
import { useNavigate } from 'react-router-dom';
import { MoveLeft, Search, Plus, X, Check, Clapperboard, Monitor, Music, PersonStanding, Shapes } from 'lucide-react';
import { Button, Stepper, Toast } from '../components/ui';
import { useTierListStore } from '../stores/tierListStore';
import { useCategoryStore } from '../stores/categoryStore';
import { SportsSearchType } from '../constants/searchType';
import type { SearchResponse } from '../models/search';
import api from '../lib/api';

const iconMap = {
  movies: Clapperboard,
  series: Monitor,
  music: Music,
  sport: PersonStanding,
};

interface AddedItem {
  key: string;
  name: string;
  externalId: string | null;
  externalSource: string | null;
  imageUrl: string | null;
}

const STEPS = [
  { number: 1, label: 'Setup' },
  { number: 2, label: 'Add Items' },
  { number: 3, label: 'Ready' },
];

const SPORTS_TYPES = [SportsSearchType.All, SportsSearchType.Player, SportsSearchType.Team];
let idCounter = 0;
const uniqueKey = () => `added-${++idCounter}-${Date.now()}`;

const CreateList = () => {
  const navigate = useNavigate();
  const { create } = useTierListStore();
  const { categories, fetchCategories } = useCategoryStore();

  const [step, setStep] = useState(0);
  const [listName, setListName] = useState('');
  const [selectedCategory, setSelectedCategory] = useState<number | null>(null);
  const [searchQuery, setSearchQuery] = useState('');
  const [searchResults, setSearchResults] = useState<SearchResponse[]>([]);
  const [searchLoading, setSearchLoading] = useState(false);
  const [addedItems, setAddedItems] = useState<AddedItem[]>([]);
  const [toastMsg, setToastMsg] = useState<string | null>(null);
  const [sportsType, setSportsType] = useState<string>(SportsSearchType.All);
  const debounceRef = useRef<ReturnType<typeof setTimeout> | undefined>(undefined);

  useEffect(() => {
    fetchCategories();
  }, [fetchCategories]);

  const selectedCat = useMemo(
    () => categories.find((c) => c.id === selectedCategory),
    [categories, selectedCategory]
  );
  const isSports = selectedCat?.slug === 'sport';
  const categoryLabel = selectedCat?.name || '';

  useEffect(() => {
    if (debounceRef.current) clearTimeout(debounceRef.current);
    if (!searchQuery.trim() || step !== 1) {
      return;
    }

    const controller = new AbortController();

    debounceRef.current = setTimeout(async () => {
      setSearchLoading(true);
      try {
        const params: Record<string, string> = {
          query: searchQuery,
          category: selectedCat?.name ?? ''
        };
        if (isSports && sportsType !== SportsSearchType.All) {
          params.type = sportsType;
        }
        const results = await api.get<SearchResponse[]>('/search', params);
        setSearchResults(results ?? []);
      } catch (err) {
        if ((err as Error).name !== 'AbortError') setSearchResults([]);
      } finally {
        setSearchLoading(false);
      }
    }, 400);

    return () => {
      clearTimeout(debounceRef.current);
      controller.abort();
    };
  }, [searchQuery, step, selectedCat?.slug, selectedCat?.name, isSports, sportsType]);

  const filteredResults = searchResults.filter(
    (r) => !addedItems.some((a) => a.externalId === r.externalId && a.externalSource === r.externalSource),
  );

  const handleAddItem = useCallback((item: SearchResponse) => {
    setAddedItems((prev) => [
      ...prev,
      {
        key: uniqueKey(),
        name: item.name ?? 'Unknown',
        externalId: item.externalId,
        externalSource: item.externalSource,
        imageUrl: item.imageUrl,
      },
    ]);
  }, []);

  const handleRemoveItem = useCallback((key: string) => {
    setAddedItems((prev) => prev.filter((a) => a.key !== key));
  }, []);

  const canNextStep = () => {
    if (step === 0) return listName.trim().length > 0 && selectedCategory !== null;
    if (step === 1) return addedItems.length >= 2;
    return true;
  };

  const handleNext = () => {
    if (step === 2) {
      create({
        name: listName,
        categoryId: selectedCategory!,
        items: addedItems.map((a) => ({
          name: a.name,
          imageUrl: a.imageUrl ?? undefined,
          externalId: a.externalId,
          externalSource: a.externalSource,
        })),
      })
        .then((list) => navigate(`/battle/${list.id}`))
        .catch(() => setToastMsg('Could not create list. Check console for details.'));
      return;
    }
    setStep((s) => s + 1);
  };

  return (
    <div className="min-h-screen bg-background">
      <div className="max-w-xl mx-auto px-6 py-8 sm:py-12">
        <button
          onClick={() => (step > 0 ? setStep((s) => s - 1) : navigate('/'))}
          className="cursor-pointer text-muted hover:text-foreground transition-colors mb-6 inline-flex items-center gap-1.5 font-body text-sm"
        >
          <MoveLeft className="w-4 h-4" />
          {step > 0 ? 'BACK' : 'HOME'}
        </button>

        <h1 className="font-display font-extrabold text-4xl text-foreground text-center mb-8">
          CREATE A NEW LIST
        </h1>

        <Stepper steps={STEPS} currentStep={step} />

        <div className="mt-10 min-h-[300px]">
          {step === 0 && (
            <div className="space-y-8 animate-fade-in-up">
              <div>
                <label className="font-body text-sm font-medium text-foreground mb-2 block">
                  List Name
                </label>
                <input
                  type="text"
                  value={listName}
                  onChange={(e) => setListName(e.target.value)}
                  placeholder="e.g. Best Kdrama of All Time"
                  className="w-full bg-surface border border-border rounded-xl px-4 py-3.5 font-body text-foreground placeholder:text-muted/50 outline-none focus:border-primary/60 focus:ring-1 focus:ring-primary/30 transition-all duration-200"
                  autoFocus
                />
              </div>

              <div>
                <label className="font-body text-sm font-medium text-foreground mb-3 block">
                  Category
                </label>
                <div className="grid grid-cols-2 gap-3">
                  {categories.map((cat) => {
                    const selected = selectedCategory === cat.id;
                    const Icon = iconMap[cat.slug as keyof typeof iconMap] ?? Shapes;

                    return (
                      <button
                        key={cat.id}
                        onClick={() => { setSelectedCategory(cat.id); setSportsType(SportsSearchType.All); }}
                        className={`cursor-pointer flex flex-col items-center gap-2 rounded-xl border-2 p-5 transition-all duration-200 ${
                          selected
                            ? 'border-primary bg-primary/10'
                            : 'border-border bg-surface hover:border-primary/40 hover:bg-surface-hover'
                        }`}
                      >
                        <Icon className={`w-6 h-6 ${selected ? 'text-primary' : 'text-muted'}`} />
                        <span
                          className={`font-body text-sm font-medium ${selected ? 'text-primary' : 'text-foreground'}`}
                        >
                          {cat.name}
                        </span>
                      </button>
                    );
                  })}
                </div>
              </div>
            </div>
          )}

          {step === 1 && (
            <div className="space-y-6 animate-fade-in-up">
              {isSports && (
                <div className="flex gap-2">
                  {SPORTS_TYPES.map((t) => (
                    <button
                      key={t}
                      onClick={() => setSportsType(t)}
                      className={`px-4 py-2 rounded-lg border text-xs font-body font-semibold transition-all ${
                        sportsType === t
                          ? 'border-primary bg-primary/10 text-primary'
                          : 'border-border bg-surface text-muted hover:border-primary/40'
                      }`}
                    >
                      {t.toUpperCase()}
                    </button>
                  ))}
                </div>
              )}

              <div className="relative">
                <Search className="absolute left-3.5 top-1/2 -translate-y-1/2 w-4 h-4 text-muted" />
                <input
                  type="text"
                  value={searchQuery}
                  onChange={(e) => setSearchQuery(e.target.value)}
                  placeholder={`Search by title${categoryLabel ? ` (${categoryLabel})` : '...'}`}
                  className="w-full bg-surface border border-border rounded-xl pl-10 pr-4 py-3 font-body text-foreground placeholder:text-muted/50 outline-none focus:border-primary/60 focus:ring-1 focus:ring-primary/30 transition-all duration-200"
                  autoFocus
                />
              </div>

              {searchQuery.trim() && (
                <div className="space-y-1 max-h-64 overflow-y-auto rounded-xl border border-border/50 bg-surface/50 p-2">
                  {searchLoading ? (
                    <p className="text-center py-8 font-body text-sm text-muted">Searching...</p>
                  ) : filteredResults.length === 0 ? (
                    <p className="text-center py-8 font-body text-sm text-muted">No results found</p>
                  ) : (
                    filteredResults.map((item, i) => (
                      <div
                        key={`${item.externalSource ?? 'src'}-${item.externalId ?? i}`}
                        className="flex items-center gap-3 px-3 py-2.5 rounded-lg hover:bg-surface-hover transition-colors group"
                      >
                        <div className="w-9 h-9 rounded-lg bg-linear-to-br from-secondary/30 to-secondary/10 flex items-center justify-center shrink-0 overflow-hidden">
                          {item.imageUrl ? (
                            <img src={item.imageUrl} alt="" className="w-full h-full object-cover" />
                          ) : (
                            <span className="font-display font-bold text-sm text-foreground">
                              {(item.name ?? '?').charAt(0)}
                            </span>
                          )}
                        </div>
                        <div className="flex-1 min-w-0">
                          <p className="font-body text-sm text-foreground truncate">{item.name}</p>
                          <p className="font-body text-xs text-muted">{item.externalSource}</p>
                        </div>
                        <button
                          onClick={() => handleAddItem(item)}
                          className="cursor-pointer flex items-center gap-1 text-xs font-body font-semibold text-primary hover:text-primary/80 transition-colors shrink-0"
                        >
                          <Plus className="w-3.5 h-3.5" />
                          ADD
                        </button>
                      </div>
                    ))
                  )}
                </div>
              )}

              <div>
                <div className="flex items-center justify-between mb-3">
                  <span className="font-body text-sm font-medium text-foreground">
                    Added Items
                  </span>
                  <span className="font-mono text-xs text-muted">{addedItems.length} selected</span>
                </div>
                {addedItems.length === 0 ? (
                  <p className="font-body text-sm text-muted/60 py-6 text-center border border-dashed border-border rounded-xl">
                    Add at least 2 items to continue
                  </p>
                ) : (
                  <div className="flex flex-wrap gap-2">
                    {addedItems.map((item) => (
                      <span
                        key={item.key}
                        className="inline-flex items-center gap-1.5 px-3 py-1.5 rounded-full bg-primary/10 border border-primary/20 text-sm font-body text-primary"
                      >
                        {item.name}
                        <button onClick={() => handleRemoveItem(item.key)} className="hover:text-lose transition-colors">
                          <X className="w-3.5 h-3.5" />
                        </button>
                      </span>
                    ))}
                  </div>
                )}
              </div>
            </div>
          )}

          {step === 2 && (
            <div className="space-y-6 animate-fade-in-up">
              <div className="rounded-xl border border-primary/20 bg-primary/5 p-6 text-center">
                <div className="w-14 h-14 rounded-full bg-primary/20 flex items-center justify-center mx-auto mb-4">
                  <Check className="w-7 h-7 text-primary" />
                </div>
                <h2 className="font-display font-extrabold text-2xl text-foreground mb-1">
                  Ready to Launch
                </h2>
                <p className="font-body text-sm text-muted">
                  Your list is ready for battle!
                </p>
              </div>

              <div className="space-y-3 rounded-xl border border-border bg-surface p-5">
                <DetailRow label="List Name" value={listName} />
                <DetailRow
                  label="Category"
                  value={categories.find((c) => c.id === selectedCategory)?.name || ''}
                />
                <DetailRow label="Items" value={`${addedItems.length} items`} />
              </div>

              <div className="flex flex-wrap gap-2">
                {addedItems.map((item) => (
                  <span
                    key={item.key}
                    className="inline-flex items-center px-2.5 py-1 rounded-full bg-border/50 text-xs font-body text-muted"
                  >
                    {item.name}
                  </span>
                ))}
              </div>
            </div>
          )}
        </div>

        <div className="mt-10 space-y-4">
          <Button
            as="button"
            variant="primary"
            size="lg"
            className="w-full justify-center"
            disabled={!canNextStep()}
            onClick={handleNext}
          >
            {step < 2 ? 'NEXT: ' + STEPS[step + 1].label.toUpperCase() : 'LAUNCH LIST →'}
          </Button>

          <p className="text-center font-body text-xs text-muted/60">
            No account needed. Your list is saved to this browser session.
          </p>
        </div>
      </div>

      <Toast message={toastMsg} onClose={() => setToastMsg(null)} />
    </div>
  );
};

const DetailRow = ({ label, value }: { label: string; value: string }) => (
  <div className="flex justify-between items-center">
    <span className="font-body text-sm text-muted">{label}</span>
    <span className="font-body text-sm text-foreground font-medium">{value}</span>
  </div>
);

export default CreateList;
