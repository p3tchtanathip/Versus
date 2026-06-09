interface ProgressBarProps {
  value: number;
  max?: number;
  color?: 'primary' | 'win' | 'lose' | 'secondary';
  size?: 'sm' | 'md';
  showLabel?: boolean;
  className?: string;
}

const colorStyles: Record<string, string> = {
  primary: 'bg-primary',
  win: 'bg-win',
  lose: 'bg-lose',
  secondary: 'bg-secondary',
};

const sizeStyles: Record<string, string> = {
  sm: 'h-1.5',
  md: 'h-2.5',
};

const ProgressBar = ({
  value,
  max = 100,
  color = 'primary',
  size = 'md',
  showLabel = false,
  className = '',
}: ProgressBarProps) => {
  const pct = Math.min(Math.max((value / max) * 100, 0), 100);

  return (
    <div className={`flex items-center gap-2 ${className}`}>
      <div className={`flex-1 rounded-full bg-border overflow-hidden ${sizeStyles[size]}`}>
        <div
          className={`h-full rounded-full transition-all duration-500 ease-out ${colorStyles[color]}`}
          style={{ width: `${pct}%` }}
        />
      </div>
      {showLabel && (
        <span className="text-xs font-mono text-muted tabular-nums">
          {Math.round(pct)}%
        </span>
      )}
    </div>
  );
};

export default ProgressBar;
