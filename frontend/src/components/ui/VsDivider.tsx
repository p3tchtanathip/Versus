const VsDivider = () => (
  <div className="flex items-center justify-center px-4 sm:px-8">
    <div className="relative flex items-center justify-center">
      <div className="absolute w-16 h-16 sm:w-24 sm:h-24 rounded-full bg-primary/10 blur-2xl animate-pulse" />
      <div className="relative w-14 h-14 sm:w-20 sm:h-20 rounded-full bg-background border-2 border-primary flex items-center justify-center shadow-[0_0_30px_rgba(232,255,71,0.3)]">
        <span className="font-display font-extrabold text-primary text-xl sm:text-3xl leading-none">
          VS
        </span>
      </div>
    </div>
  </div>
);

export default VsDivider;
