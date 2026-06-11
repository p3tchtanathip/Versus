const VsDivider = () => (
  <div className="flex items-center justify-center px-1 sm:px-8">
    <div className="relative flex items-center justify-center">
      <div className="absolute w-10 h-10 sm:w-24 sm:h-24 rounded-full bg-primary/10 blur-xl sm:blur-2xl animate-pulse" />
      <div className="relative w-10 h-10 sm:w-20 sm:h-20 rounded-full bg-background border-2 border-primary flex items-center justify-center shadow-[0_0_15px_rgba(232,255,71,0.3)] sm:shadow-[0_0_30px_rgba(232,255,71,0.3)]">
        <span className="font-display font-extrabold text-primary text-sm sm:text-3xl leading-none">
          VS
        </span>
      </div>
    </div>
  </div>
);

export default VsDivider;
