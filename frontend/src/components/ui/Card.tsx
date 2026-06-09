import type { HTMLAttributes, ReactNode } from 'react';

interface CardProps extends HTMLAttributes<HTMLDivElement> {
  children: ReactNode;
  hoverable?: boolean;
  className?: string;
}

const Card = ({ children, hoverable = false, className = '', ...props }: CardProps) => {
  const classes = [
    'rounded-xl border border-border bg-surface p-6 transition-all duration-300',
    hoverable &&
      'hover:border-primary/40 hover:shadow-[0_0_30px_-5px_rgba(232,255,71,0.15)] hover:scale-[1.02] hover:bg-surface-hover',
    className,
  ]
    .filter(Boolean)
    .join(' ');

  return (
    <div className={classes} {...props}>
      {children}
    </div>
  );
};

export default Card;
