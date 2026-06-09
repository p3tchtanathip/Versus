import type { ImgHTMLAttributes } from 'react';

interface AvatarProps extends ImgHTMLAttributes<HTMLImageElement> {
  name?: string;
  size?: 'sm' | 'md' | 'lg' | 'xl';
  className?: string;
}

const sizeMap: Record<string, string> = {
  sm: 'w-8 h-8 text-xs',
  md: 'w-10 h-10 text-sm',
  lg: 'w-14 h-14 text-lg',
  xl: 'w-20 h-20 text-2xl',
};

const Avatar = ({ src, alt = '', name, size = 'md', className = '', ...imgProps }: AvatarProps) => {
  const initial = name ? name.charAt(0).toUpperCase() : '?';

  if (src) {
    return (
      <img
        src={src}
        alt={alt || name || 'avatar'}
        className={`rounded-full object-cover ${sizeMap[size]} ${className}`}
        {...imgProps}
      />
    );
  }

  return (
    <div
      className={`rounded-full bg-border flex items-center justify-center font-display font-bold text-muted ${sizeMap[size]} ${className}`}
      aria-label={name || 'avatar placeholder'}
    >
      {initial}
    </div>
  );
};

export default Avatar;
