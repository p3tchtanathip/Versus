import { useState, useCallback, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { MoveLeft, Copy, ChevronDown, ChevronUp, Sword } from 'lucide-react';
import { Button, Toast, TierRow, HistoryRow } from '../components/ui';
import { useMatchStore } from '../stores/matchStore';
import { useTierListStore } from '../stores/tierListStore';

const Results = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const { currentList } = useTierListStore();
  const { results, history, loading, fetchResults, fetchHistory } = useMatchStore();

  const [historyOpen, setHistoryOpen] = useState(false);
  const [toastMsg, setToastMsg] = useState<string | null>(null);

  useEffect(() => {
    fetchResults(id!);
    fetchHistory(id!);
  }, [id, fetchResults, fetchHistory]);

  const handleShare = useCallback(() => {
    navigator.clipboard.writeText(window.location.href);
    setToastMsg('Link copied!');
  }, []);

  const tiers = results?.tiers ?? [];
  const lastMatch = history.length > 0 ? history[history.length - 1] : null;

  return (
    <div className="min-h-screen bg-background">
      <header className="border-b border-border/50 px-6 py-6 sm:py-8">
        <button onClick={() => navigate('/')} className="cursor-pointer text-muted hover:text-foreground transition-colors mb-4 inline-flex items-center gap-1.5 font-body text-sm">
          <MoveLeft className="w-4 h-4" />
          BACK
        </button>

        <div className="max-w-5xl mx-auto">
          <h1 className="font-display font-extrabold text-4xl sm:text-5xl text-foreground leading-tight">
            {currentList?.name}
          </h1>

          <div className="flex flex-wrap items-center gap-x-5 gap-y-2 mt-3 text-sm font-body text-muted">
            <span>{history.length} total matches</span>
            <span className="w-1 h-1 rounded-full bg-border" />
            <span>{tiers.reduce((s, t) => s + t.items.length, 0)} items</span>
            {lastMatch && (
              <>
                <span className="w-1 h-1 rounded-full bg-border" />
                <span>Last vote: {timeAgo(lastMatch.playedAt)}</span>
              </>
            )}
          </div>

          <div className="flex gap-3 mt-6">
            <Button as="link" to={`/battle/${id}`} variant="primary" size="md">
              <Sword className="w-4 h-4" />
              BATTLE MORE
            </Button>
            <Button as="button" variant="ghost" size="md" onClick={handleShare}>
              <Copy className="w-4 h-4" />
              SHARE LIST
            </Button>
          </div>
        </div>
      </header>

      <section className="max-w-5xl mx-auto px-6 py-10">
        {loading && tiers.length === 0 ? (
          <div className="flex items-center justify-center py-20">
            <div className="w-8 h-8 border-2 border-primary/50 border-t-primary rounded-full animate-spin" />
          </div>
        ) : tiers.length === 0 ? (
          <div className="text-center py-20">
            <p className="font-body text-muted">No results yet. Go battle some items!</p>
          </div>
        ) : (
          tiers.map((tier, i) => <TierRow key={tier.tierLabel} tier={tier} index={i} />)
        )}

        <div className="mt-12 border-t border-border/50 pt-8">
          <button
            onClick={() => {
              if (!historyOpen) fetchHistory(id!);
              setHistoryOpen(!historyOpen);
            }}
            className="flex items-center gap-2 font-display font-bold text-lg text-foreground hover:text-primary transition-colors w-full text-left"
          >
            {historyOpen ? <ChevronUp className="w-5 h-5" /> : <ChevronDown className="w-5 h-5" />}
            MATCH HISTORY
            <span className="font-body text-xs text-muted font-normal ml-1">(Last {history.length})</span>
          </button>

          {historyOpen && (
            <div className="mt-4 space-y-2">
              {history.length === 0 ? (
                <p className="text-center py-8 font-body text-sm text-muted">No matches played yet.</p>
              ) : (
                [...history].reverse().map((match, i) => (
                  <HistoryRow key={match.id} match={match} index={i} />
                ))
              )}
            </div>
          )}
        </div>
      </section>

      <Toast message={toastMsg} onClose={() => setToastMsg(null)} />
    </div>
  );
};

function timeAgo(dateStr: string): string {
  const diff = Date.now() - new Date(dateStr).getTime();
  const mins = Math.floor(diff / 60000);
  if (mins < 60) return `${mins}m ago`;
  const hours = Math.floor(mins / 60);
  if (hours < 24) return `${hours}h ago`;
  const days = Math.floor(hours / 24);
  return `${days}d ago`;
}

export default Results;
