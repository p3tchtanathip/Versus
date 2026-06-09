import type { ButtonHTMLAttributes, ReactNode } from 'react';
import { Link } from 'react-router-dom';

type ButtonVariant = 'primary' | 'ghost';
type ButtonSize = 'sm' | 'md' | 'lg';

interface ButtonBaseProps {
  variant?: ButtonVariant;
  size?: ButtonSize;
  children: ReactNode;
  className?: string;
}

interface ButtonAsButton extends ButtonBaseProps, Omit<ButtonHTMLAttributes<HTMLButtonElement>, 'children'> {
  as?: 'button';
  to?: never;
}

interface ButtonAsLink extends ButtonBaseProps {
  as: 'link';
  to: string;
}

type ButtonProps = ButtonAsButton | ButtonAsLink;

const variantStyles: Record<ButtonVariant, string> = {
  primary:
    'bg-primary text-black font-bold hover:brightness-110 active:brightness-90 border-2 border-primary',
  ghost:
    'bg-transparent text-foreground font-medium hover:bg-white/5 active:bg-white/10 border-2 border-foreground/30 hover:border-foreground/60',
};

const sizeStyles: Record<ButtonSize, string> = {
  sm: 'px-4 py-1.5 text-sm',
  md: 'px-6 py-2.5 text-base',
  lg: 'px-8 py-3.5 text-lg',
};

const Button = (props: ButtonProps) => {
  const { variant = 'primary', size = 'md', children, className = '' } = props;

  const classes = [
    'inline-flex items-center justify-center gap-2 rounded-lg font-body transition-all duration-200 cursor-pointer select-none',
    variantStyles[variant],
    sizeStyles[size],
    className,
  ]
    .filter(Boolean)
    .join(' ');

  if (props.as === 'link') {
    return (
      <Link to={props.to} className={classes}>
        {children}
      </Link>
    );
  }

  return (
    <button
      className={classes}
      onClick={(props as ButtonAsButton).onClick}
      disabled={(props as ButtonAsButton).disabled}
      type={(props as ButtonAsButton).type}
    >
      {children}
    </button>
  );
};

export default Button;
