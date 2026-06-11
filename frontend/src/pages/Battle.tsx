import { useState, useCallback, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { ArrowRight, MoveLeft } from 'lucide-react';
import { Button, MatchCard, VsDivider, ProgressBar } from '../components/ui';
import { useMatchStore } from '../stores/matchStore';
import { useTierListStore } from '../stores/tierListStore';

type VoteState = 'voting' | 'voted' | 'transitioning';

interface VoteResult {
  leftDelta: number;
  rightDelta: number;
}

const Battle = () => {
  const { id } = useParams();
  const navigate = useNavigate();

  const { loading, currentMatch, progress, fetchNextMatch, submitMatch } = useMatchStore();
  const { currentList, fetchByIdIfNeeded } = useTierListStore();

  const [voteState, setVoteState] = useState<VoteState>('voting');
  const [winner, setWinner] = useState<'left' | 'right' | null>(null);
  const [voteResult, setVoteResult] = useState<VoteResult | null>(null);
  const [animKey, setAnimKey] = useState(0);

  useEffect(() => {
    fetchByIdIfNeeded(id!);
  }, [id, fetchByIdIfNeeded]);

  useEffect(() => {
    fetchNextMatch(id!);
  }, [id, fetchNextMatch]);

  useEffect(() => {
    if (!loading && !currentMatch && progress && progress.played >= progress.total) {
      navigate(`/results/${id}`);
    }
  }, [currentMatch, loading, progress, id, navigate]);

  const handleVote = useCallback(
    async (side: 'left' | 'right') => {
      if (voteState !== 'voting' || !currentMatch) return;

      setWinner(side);
      setVoteState('voted');

      const winnerItem = side === 'left' ? currentMatch.itemA : currentMatch.itemB;
      const loserItem  = side === 'left' ? currentMatch.itemB : currentMatch.itemA;

      try {
        const result = await submitMatch({
          tierListId: id!,
          winnerId: winnerItem.id,
          loserId: loserItem.id,
        });

        setVoteResult(
          side === 'left'
            ? { leftDelta: result.winnerDelta, rightDelta: result.loserDelta }
            : { leftDelta: result.loserDelta,  rightDelta: result.winnerDelta }
        );
      } catch {
        setVoteResult(null);
      }

      setTimeout(() => {
        setVoteState('transitioning');
        setTimeout(() => {
          setAnimKey(k => k + 1);
          setWinner(null);
          setVoteResult(null);
          setVoteState('voting');
          fetchNextMatch(id!);
        }, 300);
      }, 1500);
    },
    [voteState, currentMatch, id, submitMatch, fetchNextMatch],
  );

  const getCardStatus = (side: 'left' | 'right') => {
    if (voteState === 'voting' || winner === null) return 'voting' as const;
    return side === winner ? 'winner' : 'loser';
  };

  const cardAnimClass = (side: 'left' | 'right') => {
    if (voteState === 'transitioning') return 'animate-fade-out-down';
    return side === 'left' ? 'animate-slide-in-left' : 'animate-slide-in-right';
  };

  const played  = progress?.played ?? 0;
  const total   = progress?.total  ?? 0;
  const percent = total > 0 ? Math.round((played / total) * 100) : 0;

  return (
    <div className="min-h-screen bg-background flex flex-col">
      <header className="flex items-center justify-between gap-2 sm:gap-3 px-4 sm:px-6 py-4 border-b border-border/50">
        <div className="flex items-center gap-2 sm:gap-3 min-w-0 flex-1 sm:flex-none">
          <button
            onClick={() => navigate('/')}
            className="text-muted hover:text-foreground transition-colors cursor-pointer shrink-0"
          >
            <MoveLeft className="w-5 h-5" />
          </button>

          <h1 className="font-display font-extrabold text-xl text-foreground truncate">
            {currentList?.name}
          </h1>
        </div>

        <div className="hidden md:flex flex-col px-4 pb-3 pt-2 border-b border-border/50 flex-1 max-w-sm mx-auto">
          <div className="flex items-center justify-between mb-1.5">
            <span className="font-body text-[11px] text-muted">{played} battles fought</span>
            <span className="font-mono text-[11px] text-muted">{percent}%</span>
          </div>
          <ProgressBar value={played} max={total} size="sm" />
        </div>

        <div className="shrink-0 ml-auto md:ml-0">
          <Button
            variant="ghost"
            size="sm"
            onClick={() => navigate(`/results/${id}`)}
          >
            VIEW RESULTS
            <ArrowRight className="w-4 h-4" />
          </Button>
        </div>
      </header>

      <div className="md:hidden px-4 pb-3 pt-2 border-b border-border/50">
        <div className="flex items-center justify-between mb-1.5">
          <span className="font-body text-[11px] text-muted">{played} battles fought</span>
          <span className="font-mono text-[11px] text-muted">{percent}%</span>
        </div>
        <ProgressBar value={played} max={total} size="sm" />
      </div>

      <main className="flex-1 flex items-center justify-center px-2 sm:px-8 py-4 sm:py-8">
        {loading && !currentMatch ? (
          <div className="flex items-center gap-2 text-muted font-body">
            <div className="w-5 h-5 border-2 border-primary/50 border-t-primary rounded-full animate-spin" />
            Loading match...
          </div>
        ) : currentMatch ? (
          <div key={animKey} className="flex items-center justify-center w-full max-w-4xl mx-auto">
            <div className={cardAnimClass('left')} style={{ flex: 1, display: 'flex', justifyContent: 'flex-end' }}>
              <MatchCard
                item={currentMatch.itemA}
                status={getCardStatus('left')}
                delta={voteState !== 'voting' ? voteResult?.leftDelta : undefined}
                onVote={() => handleVote('left')}
              />
            </div>

            <VsDivider />

            <div className={cardAnimClass('right')} style={{ flex: 1, display: 'flex', justifyContent: 'flex-start' }}>
              <MatchCard
                item={currentMatch.itemB}
                status={getCardStatus('right')}
                delta={voteState !== 'voting' ? voteResult?.rightDelta : undefined}
                onVote={() => handleVote('right')}
              />
            </div>
          </div>
        ) : null}
      </main>

      <footer className="py-12 text-center">
        <p className="font-body text-xs text-muted/60">No need to finish - results update after every vote</p>
      </footer>
    </div>
  );
};

export default Battle;