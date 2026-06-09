interface EloDeltaChipProps {
  delta: number;
  size?: 'sm' | 'md';
}

const EloDeltaChip = ({ delta, size = 'md' }: EloDeltaChipProps) => {
  const isPositive = delta >= 0;
  const sign = isPositive ? '+' : '';
  const colorClass = isPositive ? 'text-win bg-win/15' : 'text-lose bg-lose/15';
  const sizeClass = size === 'sm' ? 'text-xs px-1.5 py-0.5' : 'text-sm px-2.5 py-1';

  return (
    <span
      className={`inline-flex items-center gap-0.5 rounded-md font-mono font-medium leading-none ${colorClass} ${sizeClass}`}
    >
      {sign}{delta}
    </span>
  );
};

export default EloDeltaChip;
