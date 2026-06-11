import EloDeltaChip from './EloDeltaChip';
import type { MatchResponse } from '../../models/match';

function timeAgo(dateStr: string): string {
  const diff = Date.now() - new Date(dateStr).getTime();
  const mins = Math.floor(diff / 60000);
  if (mins < 60) return `${mins}m ago`;
  const hours = Math.floor(mins / 60);
  if (hours < 24) return `${hours}h ago`;
  const days = Math.floor(hours / 24);
  return `${days}d ago`;
}

export const HistoryRow = ({ match, index }: { match: MatchResponse; index: number }) => (
  <div
    className="flex items-center gap-3 py-3 px-4 rounded-xl border border-border/50 bg-surface/30 animate-fade-in-up"
    style={{ animationDelay: `${index * 50}ms` }}
  >
    <div className="w-7 h-7 rounded-full bg-gradient-to-br from-secondary/30 to-secondary/10 flex items-center justify-center shrink-0">
      <span className="font-display font-bold text-[10px] text-foreground">{match.winner.name.charAt(0)}</span>
    </div>
    <span className="font-body text-sm text-foreground font-medium truncate max-w-[80px] sm:max-w-[180px]">
      {match.winner.name}
    </span>
    <span className="font-body text-xs text-muted shrink-0">beat</span>
    <span className="font-body text-sm text-foreground truncate max-w-[80px] sm:max-w-[180px]">
      {match.loser.name}
    </span>
    <div className="ml-auto flex items-center gap-3 shrink-0">
      <EloDeltaChip delta={match.winnerDelta} size="sm" />
      <span className="font-mono text-[11px] text-muted/60 tabular-nums">{timeAgo(match.playedAt)}</span>
    </div>
  </div>
);
