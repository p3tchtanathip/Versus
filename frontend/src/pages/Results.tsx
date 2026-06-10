import { useState, useCallback, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { MoveLeft, Copy, ChevronDown, ChevronUp, Sword, RefreshCw } from 'lucide-react';
import { Button, Toast, TierRow, HistoryRow } from '../components/ui';
import { useMatchStore } from '../stores/matchStore';
import { useTierListStore } from '../stores/tierListStore';
import { timeAgo } from '../utils/time';
import api from '../lib/api';

const Results = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const { currentList, fetchById } = useTierListStore();
  const { results, history, resultsLoading, fetchResults, fetchHistory } = useMatchStore();

  const [historyOpen, setHistoryOpen] = useState(false);
  const [toastMsg, setToastMsg] = useState<string | null>(null);

  useEffect(() => {
    fetchById(id!);
    fetchResults(id!);
    fetchHistory(id!);
  }, [id, fetchById, fetchResults, fetchHistory]);

  const handleShare = useCallback(() => {
    navigator.clipboard.writeText(window.location.href);
    setToastMsg('Link copied!');
  }, []);

  const handlePlayAgain = useCallback(async () => {
    await api.post(`/lists/${id}/reset-session`, {});
    navigate(`/battle/${id}`);
  }, [id, navigate]);

  const tiers = results?.tiers ?? [];
  const lastMatch = history.length > 0 ? history[0] : null;
  const isComplete = currentList ? currentList.playedCount >= currentList.totalPairs : false;

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
            <span>{currentList?.matchCount} total matches</span>
            <span className="w-1 h-1 rounded-full bg-border" />
            <span>{currentList?.itemCount} items</span>
            {lastMatch && (
              <>
                <span className="w-1 h-1 rounded-full bg-border" />
                <span>Last vote: {timeAgo(lastMatch.playedAt)}</span>
              </>
            )}
          </div>

          <div className="flex gap-3 mt-6">
            {isComplete ? (
              <Button as="button" variant="primary" size="md" onClick={handlePlayAgain}>
                <RefreshCw className="w-4 h-4" />
                PLAY AGAIN
              </Button>
            ) : (
              <Button as="link" to={`/battle/${id}`} variant="primary" size="md">
                <Sword className="w-4 h-4" />
                BATTLE MORE
              </Button>
            )}
            <Button as="button" variant="ghost" size="md" onClick={handleShare}>
              <Copy className="w-4 h-4" />
              SHARE LIST
            </Button>
          </div>
        </div>
      </header>

      <section className="max-w-5xl mx-auto px-6 py-10">
        {resultsLoading && tiers.length === 0 ? (
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
                [...history].map((match, i) => (
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

export default Results;
