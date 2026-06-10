import { useEffect, useRef, useCallback } from 'react';
import { useNavigate } from 'react-router-dom';
import { useTierListStore } from '../stores/tierListStore';
import { useCategoryStore } from '../stores/categoryStore';
import { Button, Card, CategoryBadge } from '../components/ui';
import { MoveRight, Star } from 'lucide-react';
import type { TierListResponse } from '../models/tierList';
import { timeAgoDays, formatNumber } from '../utils/time';

const Home = () => {
  const navigate = useNavigate();
  const exploreRef = useRef<HTMLDivElement>(null);

  const { lists, loading: listsLoading, fetchLists, fetchById } = useTierListStore();
  const { fetchCategories } = useCategoryStore();

  useEffect(() => {
    fetchLists();
    fetchCategories();
  }, [fetchLists, fetchCategories]);

  const scrollToExplore = useCallback(() => {
    exploreRef.current?.scrollIntoView({ behavior: 'smooth' });
  }, []);

  const trendingLists = lists?.items?.slice(0, 9);

  const handleViewList = (id: string) => {
    fetchById(id);
    navigate(`/battle/${id}`);
  };

  return (
    <div className="min-h-screen bg-background">
      {/* ── Hero ── */}
      <section className="relative min-h-[60vh] flex items-center justify-center overflow-hidden px-6 py-24">
        <div
          className="pointer-events-none absolute inset-0 opacity-[0.07]"
          style={{
            backgroundImage: [
              'linear-gradient(45deg, rgba(232,255,71,0.15) 1px, transparent 1px)',
              'linear-gradient(-45deg, rgba(232,255,71,0.15) 1px, transparent 1px)',
            ].join(', '),
            backgroundSize: '60px 60px',
            animation: 'grid-shift 8s linear infinite',
          }}
        />

        <div className="pointer-events-none absolute inset-0 bg-linear-to-b from-primary/5 via-transparent to-background" />

        <div className="relative z-10 text-center max-w-3xl mx-auto">
          <h1 className="font-display font-extrabold text-foreground text-[96px] leading-[0.9] tracking-tight mb-4">
            WHO WINS?
          </h1>
          <p className="font-body text-muted text-lg mb-10">
            Crowd-Sourced ELO Battle Rankings
          </p>
          <div className="flex items-center justify-center gap-4 flex-wrap">
            <Button
              as="link"
              to="/create"
              variant="primary"
              size="lg"
            >
              CREATE A LIST
            </Button>
            <Button
              as="button"
              variant="ghost"
              size="lg"
              onClick={scrollToExplore}
            >
              EXPLORE LISTS
            </Button>
          </div>
        </div>

        <div className="pointer-events-none absolute bottom-0 left-0 right-0 h-32 bg-linear-to-t from-background to-transparent" />
      </section>

      {/* ── Explore ── */}
      <section ref={exploreRef} className="max-w-7xl mx-auto px-6 pb-24">
        <div className="flex items-center gap-3 mb-10">
          <Star fill="var(--color-primary)" strokeWidth={0} />
          <h2 className="font-display font-bold text-2xl text-foreground tracking-wide">
            TRENDING BATTLES
          </h2>
        </div>

        {listsLoading && trendingLists?.length === 0 && (
          <div className="flex items-center justify-center py-20">
            <div className="w-8 h-8 border-2 border-primary/50 border-t-primary rounded-full animate-spin" />
          </div>
        )}

        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {trendingLists?.map((list: TierListResponse) => (
            <Card key={list.id} hoverable className="group cursor-pointer" onClick={() => handleViewList(list.id)}>
              <div className="flex items-start justify-between mb-3">
                <CategoryBadge label={list.categoryName} />
              </div>

              <h3 className="font-display font-bold text-2xl text-foreground mb-4 leading-tight">
                {list.name}
              </h3>

              <div className="relative flex items-center gap-2 mb-4">
                <div className="flex-1 aspect-3/2 rounded-lg bg-linear-to-br from-secondary/30 to-secondary/10 flex items-center justify-center overflow-hidden">
                  <span className="font-display font-bold text-muted text-sm">Item A</span>
                </div>
                <div className="absolute left-1/2 top-1/2 -translate-x-1/2 -translate-y-1/2 z-10 w-10 h-10 rounded-full bg-primary flex items-center justify-center shadow-lg shadow-primary/30">
                  <span className="font-display font-extrabold text-black text-xs leading-none">VS</span>
                </div>
                <div className="flex-1 aspect-3/2 rounded-lg bg-linear-to-tl from-primary/30 to-primary/10 flex items-center justify-center overflow-hidden">
                  <span className="font-display font-bold text-muted text-sm">Item B</span>
                </div>
              </div>

              <div className="flex items-center gap-3 text-xs font-body text-muted mb-3">
                <span>{formatNumber(list.matchCount)} matches</span>
                <span className="w-1 h-1 rounded-full bg-border" />
                <span>{list.itemCount} items</span>
                <span className="w-1 h-1 rounded-full bg-border" />
                <span>{timeAgoDays(list.createdAt)}</span>
              </div>

              <div className="flex justify-end">
                <div className="group flex items-center gap-2 text-sm font-semibold text-primary transition-colors hover:text-primary/80">
                  BATTLE NOW
                  <MoveRight className="h-4 w-4 transition-transform group-hover:translate-x-1" />
                </div>
              </div>
            </Card>
          ))}
        </div>
      </section>
    </div>
  );
};

export default Home;
