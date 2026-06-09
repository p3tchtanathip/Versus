import type { ReactNode } from 'react';

type TierLevel = 'S' | 'A' | 'B' | 'C' | 'D' | 'F';

interface TierBadgeProps {
  tier: TierLevel;
  size?: 'sm' | 'md' | 'lg';
}

const tierColors: Record<TierLevel, string> = {
  S: 'bg-tier-s text-black',
  A: 'bg-tier-a text-black',
  B: 'bg-tier-b text-white',
  C: 'bg-tier-c text-white',
  D: 'bg-tier-d text-white',
  F: 'bg-tier-f text-white',
};

const sizeStyles: Record<string, string> = {
  sm: 'w-6 h-6 text-[10px]',
  md: 'w-8 h-8 text-xs',
  lg: 'w-10 h-10 text-sm',
};

const TierBadge = ({ tier, size = 'md' }: TierBadgeProps) => (
  <span
    className={`inline-flex items-center justify-center rounded-full font-display font-extrabold leading-none ${tierColors[tier]} ${sizeStyles[size]}`}
  >
    {tier}
  </span>
);

// --- CategoryBadge ---

interface CategoryBadgeProps {
  label: string;
  className?: string;
}

const variantColors: Record<string, string> = {
  SPORT: 'bg-secondary/20 text-secondary border-secondary/30',
  SERIES: 'bg-win/20 text-win border-win/30',
  MOVIES: 'bg-primary/20 text-primary border-primary/30',
  MUSIC: 'bg-tier-b/20 text-tier-b border-tier-b/30',
  DEFAULT: 'bg-muted/20 text-muted border-muted/30',
};

const CategoryBadge = ({ label, className = '' }: CategoryBadgeProps) => {
  const colorClass = variantColors[label.toUpperCase()] || variantColors.DEFAULT;

  return (
    <span
      className={`inline-flex items-center px-2.5 py-1 rounded-full border text-[11px] font-body font-semibold uppercase tracking-wider ${colorClass} ${className}`}
    >
      {label}
    </span>
  );
};

// --- Generic Badge ---

interface BadgeProps {
  children: ReactNode;
  variant?: 'default' | 'primary' | 'secondary' | 'win' | 'lose';
  className?: string;
}

const badgeVariants: Record<string, string> = {
  default: 'bg-muted/20 text-muted',
  primary: 'bg-primary/20 text-primary',
  secondary: 'bg-secondary/20 text-secondary',
  win: 'bg-win/20 text-win',
  lose: 'bg-lose/20 text-lose',
};

const Badge = ({ children, variant = 'default', className = '' }: BadgeProps) => (
  <span
    className={`inline-flex items-center px-2.5 py-1 rounded-full text-xs font-body font-semibold ${badgeVariants[variant]} ${className}`}
  >
    {children}
  </span>
);

export { TierBadge, CategoryBadge, Badge };
export default Badge;
