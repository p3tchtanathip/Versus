import type { ReactNode } from 'react';

interface TagPillProps {
  children: ReactNode;
  color?: 'primary' | 'secondary' | 'win' | 'lose' | 'muted';
  className?: string;
}

const colorStyles: Record<string, string> = {
  primary: 'bg-primary/10 text-primary border-primary/20',
  secondary: 'bg-secondary/10 text-secondary border-secondary/20',
  win: 'bg-win/10 text-win border-win/20',
  lose: 'bg-lose/10 text-lose border-lose/20',
  muted: 'bg-muted/10 text-muted border-muted/20',
};

const TagPill = ({ children, color = 'muted', className = '' }: TagPillProps) => (
  <span
    className={`inline-flex items-center gap-1 px-2.5 py-0.5 rounded-full border text-xs font-body font-medium ${colorStyles[color]} ${className}`}
  >
    {children}
  </span>
);

export default TagPill;
