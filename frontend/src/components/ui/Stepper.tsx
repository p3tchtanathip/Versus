import { Check } from 'lucide-react';

interface Step {
  number: number;
  label: string;
}

interface StepperProps {
  steps: Step[];
  currentStep: number;
}

const Stepper = ({ steps, currentStep }: StepperProps) => (
  <div className="flex items-center justify-center">
    {steps.map((step, i) => (
      <div key={step.number} className="flex items-center">
        <div className="flex items-center gap-2">
          <div
            className={`w-8 h-8 rounded-full flex items-center justify-center font-display font-bold text-sm transition-colors ${
              i < currentStep
                ? 'bg-primary text-black'
                : i === currentStep
                ? 'bg-primary text-black ring-2 ring-primary/40'
                : 'bg-border text-muted'
            }`}
          >
            {i < currentStep ? <Check className="w-4 h-4" /> : step.number}
          </div>
          <span
            className={`font-body text-sm hidden sm:inline ${
              i === currentStep ? 'text-foreground font-medium' : 'text-muted'
            }`}
          >
            {step.label}
          </span>
        </div>
        {i < steps.length - 1 && (
          <div className={`w-8 sm:w-16 h-0.5 mx-2 ${i < currentStep ? 'bg-primary' : 'bg-border'}`} />
        )}
      </div>
    ))}
  </div>
);

export default Stepper;
