import { useState } from 'react';
import { ChevronRight } from 'lucide-react';
import { EloChartPanel } from './EloChartPanel';
import type { TierGroupResponse } from '../../models/match';

const TIER_COLORS: Record<string, string> = {
  S: '#FFD700',
  A: '#E8FF47',
  B: '#7B5EFF',
  C: '#4FC3F7',
  D: '#7A7A8C',
};

function formatElo(n: number): string {
  return n.toLocaleString();
}

export const TierRow = ({ tier, index }: { tier: TierGroupResponse; index: number }) => {
  const color = TIER_COLORS[tier.tierLabel] || '#7A7A8C';
  const [selectedItem, setSelectedItem] = useState<string | null>(null);

  return (
    <div className="animate-fade-in-up" style={{ animationDelay: `${index * 80}ms` }}>
      <div className="flex gap-4 sm:gap-6">
        <div className="flex flex-col items-center pt-1 min-w-[48px] sm:min-w-[60px]">
          <span className="font-display font-extrabold text-4xl sm:text-5xl leading-none" style={{ color }}>
            {tier.tierLabel}
          </span>
          <span className="w-1 flex-1 rounded-full mt-2" style={{ backgroundColor: color, width: 4, minHeight: 32 }} />
        </div>

        <div className="flex-1 flex flex-wrap gap-2 pb-6">
          {tier.items.map((item) => (
            <button
              key={item.id}
              onClick={() => setSelectedItem(selectedItem === item.id ? null : item.id)}
              className={`group flex items-center gap-2.5 rounded-xl border bg-surface px-3 py-2.5 transition-all duration-200 hover:border-primary/40 hover:bg-surface-hover ${
                selectedItem === item.id ? 'border-primary/50 ring-1 ring-primary/30' : 'border-border'
              }`}
            >
              <div className="w-8 h-8 rounded-full bg-gradient-to-br from-secondary/40 to-secondary/10 flex items-center justify-center shrink-0 overflow-hidden">
                {item.imageUrl ? (
                  <img src={item.imageUrl} alt={item.name} className="w-full h-full object-cover" />
                ) : (
                  <span className="font-display font-bold text-xs text-foreground">{item.name.charAt(0)}</span>
                )}
              </div>
              <span className="font-body text-sm text-foreground whitespace-nowrap">{item.name}</span>
              <span className="font-mono text-xs text-muted tabular-nums">{formatElo(item.eloRating)}</span>
              {selectedItem === item.id && (
                <ChevronRight className="w-3.5 h-3.5 text-primary ml-1" />
              )}
            </button>
          ))}
        </div>
      </div>

      {selectedItem && (
        <div className="mb-4 ml-[72px] rounded-xl border border-primary/20 bg-surface/50 p-4 animate-fade-in-up">
          <EloChartPanel itemId={selectedItem} itemName={tier.items.find(i => i.id === selectedItem)?.name || ''} />
        </div>
      )}
    </div>
  );
};
