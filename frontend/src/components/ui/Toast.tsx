import { useEffect } from 'react';
import { X } from 'lucide-react';

interface ToastProps {
  message: string | null;
  onClose: () => void;
  duration?: number;
}

const Toast = ({ message, onClose, duration = 3000 }: ToastProps) => {
  useEffect(() => {
    if (message) {
      const timer = setTimeout(onClose, duration);
      return () => clearTimeout(timer);
    }
  }, [message, duration, onClose]);

  if (!message) return null;

  return (
    <div className="fixed bottom-8 left-1/2 -translate-x-1/2 z-50 animate-fade-in-up">
      <div className="flex items-center gap-3 bg-surface border border-border/50 rounded-xl px-5 py-3 shadow-2xl backdrop-blur-sm">
        <span className="text-sm font-body text-foreground">{message}</span>
        <button onClick={onClose} className="text-muted hover:text-foreground transition-colors">
          <X className="w-4 h-4" />
        </button>
      </div>
    </div>
  );
};

export default Toast;
