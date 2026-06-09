import { useState, useEffect, useRef } from 'react';
import api from '../../lib/api';
import type { ItemEloHistoryResponse } from '../../models/item';

export const EloChartPanel = ({ itemId, itemName }: { itemId: string; itemName: string }) => {
  const [data, setData] = useState<ItemEloHistoryResponse | null>(null);
  const [hoveredIdx, setHoveredIdx] = useState<number | null>(null);
  const requestRef = useRef(0);

  useEffect(() => {
    const id = ++requestRef.current;
    api.get<ItemEloHistoryResponse>(`/items/${itemId}/elo-history`)
      .then(d => { if (id === requestRef.current) setData(d); })
      .catch(() => { if (id === requestRef.current) setData(null); });
  }, [itemId]);

  if (!data) {
    return (
      <div className="flex items-center justify-center py-6">
        <div className="w-5 h-5 border-2 border-primary/50 border-t-primary rounded-full animate-spin" />
      </div>
    );
  }

  if (data.points.length === 0) return null;

  const ratings = [data.initialRating, ...data.points.map(p => p.ratingAfter)];
  const min = Math.min(...ratings) - 50;
  const max = Math.max(...ratings) + 50;
  const range = max - min;
  const w = 400;
  const h = 120;

  const line = ratings
    .map((p, i) => {
      const x = (i / (ratings.length - 1)) * w;
      const y = h - ((p - min) / range) * h;
      return `${i === 0 ? 'M' : 'L'} ${x.toFixed(1)} ${y.toFixed(1)}`;
    })
    .join(' ');

  return (
    <div>
      <div className="flex items-center justify-between mb-3">
        <span className="font-body text-sm font-medium text-foreground">{itemName} — ELO History</span>
        <span className="font-mono text-xs text-muted">Last {data.points.length} matches</span>
      </div>
      <svg viewBox={`0 0 ${w} ${h}`} className="w-full max-w-md h-24 sm:h-28" preserveAspectRatio="xMidYMid meet">
        <defs>
          <linearGradient id="chart-fill" x1="0" y1="0" x2="0" y2="1">
            <stop offset="0%" stopColor="#E8FF47" stopOpacity="0.2" />
            <stop offset="100%" stopColor="#E8FF47" stopOpacity="0" />
          </linearGradient>
        </defs>
        <path d={`${line} L ${w} ${h} L 0 ${h} Z`} fill="url(#chart-fill)" />
        <path d={line} fill="none" stroke="#E8FF47" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round" />
        {ratings.map((p, i) => {
          const x = (i / (ratings.length - 1)) * w;
          const y = h - ((p - min) / range) * h;
          return (
            <g key={i}>
              <circle
                cx={x} cy={y}
                r={hoveredIdx === i ? 6 : 3}
                fill="#E8FF47"
                onMouseEnter={() => setHoveredIdx(i)}
                onMouseLeave={() => setHoveredIdx(null)}
                className="hidden sm:block cursor-pointer transition-all duration-150"
              />
              {hoveredIdx === i && (
                <foreignObject x={x - 30} y={y - 28} width={60} height={22}>
                  <div className="flex items-center justify-center bg-background border border-primary/40 rounded px-1.5 py-0.5">
                    <span className="font-mono text-[11px] text-primary">{p}</span>
                  </div>
                </foreignObject>
              )}
            </g>
          );
        })}
      </svg>
      <div className="flex justify-between mt-1">
        <span className="font-mono text-[10px] text-muted/60">Earliest</span>
        <span className="font-mono text-[10px] text-muted/60">Latest</span>
      </div>
    </div>
  );
};
