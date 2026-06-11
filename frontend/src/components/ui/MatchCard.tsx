import type { ItemResponse } from '../../models/item';
import { EloDeltaChip } from './index';

type CardStatus = 'voting' | 'winner' | 'loser';

interface MatchCardProps {
  item: ItemResponse;
  status: CardStatus;
  delta?: number;
  onVote: () => void;
}

const formatElo = (rating: number) => rating.toLocaleString();

const gradientFromName = (name: string) => {
  const hash = name.split('').reduce((a, c) => a + c.charCodeAt(0), 0);
  const hue1 = hash % 360;
  const hue2 = (hash * 7) % 360;
  return `linear-gradient(135deg, hsl(${hue1}, 40%, 20%), hsl(${hue2}, 30%, 12%))`;
};

const MatchCard = ({ item, status, delta, onVote }: MatchCardProps) => {
  const isWinner = status === 'winner';
  const isLoser = status === 'loser';
  const isInteractive = status === 'voting';

  const borderColor = isWinner
    ? 'border-win shadow-[0_0_30px_rgba(57,255,106,0.3)]'
    : isLoser
    ? 'border-border border-opacity-40'
    : 'border-border';

  const wrapperClasses = [
    'relative flex-1 max-w-[160px] sm:max-w-[360px] rounded-2xl border-2 overflow-hidden transition-all duration-500',
    borderColor,
    isInteractive && 'cursor-pointer hover:border-primary/60 hover:scale-[1.03] hover:shadow-[0_0_40px_rgba(232,255,71,0.15)]',
    isLoser && 'opacity-50',
    isWinner && 'animate-[glow-pulse-win_2s_ease-in-out_infinite]',
  ]
    .filter(Boolean)
    .join(' ');

  return (
    <div className={wrapperClasses} onClick={isInteractive ? onVote : undefined}>
      <DeltaBadge delta={delta} isWinner={isWinner} />

      <div
        className="aspect-[3/4] sm:aspect-[2/3] w-full flex items-center justify-center"
        style={{ background: gradientFromName(item.name) }}
      >
        {item.imageUrl ? (
          <img src={item.imageUrl} alt={item.name} className="w-full h-full object-cover" />
        ) : (
          <div className="text-center px-4 py-8">
            <div className="w-10 h-10 sm:w-20 sm:h-20 rounded-full bg-white/5 flex items-center justify-center mx-auto mb-2 sm:mb-3">
              <span className="font-display font-extrabold text-xl sm:text-3xl text-muted">
                {item.name.charAt(0)}
              </span>
            </div>
            {item.externalSource && (
              <span className="font-body text-[10px] uppercase tracking-widest text-muted/50">
                {item.externalSource}
              </span>
            )}
          </div>
        )}
      </div>

      <div className="p-2 sm:p-5 bg-surface/95 backdrop-blur-sm">
        <h3 className="font-display font-extrabold text-sm sm:text-3xl text-foreground truncate leading-tight">
          {item.name}
        </h3>
        <p className="font-mono text-xs sm:text-sm text-muted mt-0 sm:mt-1">
          {formatElo(item.eloRating)} ELO
        </p>
      </div>
    </div>
  );
};

const DeltaBadge = ({ delta, isWinner }: { delta?: number; isWinner: boolean }) => {
  if (delta === undefined) return null;

  return (
    <div
      className={`absolute top-3 right-3 z-10 ${
        isWinner ? 'animate-fade-in-up' : 'animate-fade-in-up'
      }`}
    >
      <EloDeltaChip delta={delta} size="md" />
    </div>
  );
};

export default MatchCard;
