type InputProps = {
  placeholder: string;

  value: string;

  type?: string;

  maxLength?: number;

  onKeyDown?: (e: React.KeyboardEvent<HTMLInputElement>) => void;

  onChange: (value: string) => void;
};

function Input({
  placeholder,
  value,
  type = "text",
  maxLength,
  onKeyDown,
  onChange,
}: InputProps) {
  return (
    <input
      className="
  border
  border-gray-300

  px-3
  py-2

  rounded-md

  outline-none

  focus:border-blue-500
  focus:ring-1
  focus:ring-blue-500

  transition
  duration-200
"
      placeholder={placeholder}
      value={value}
      type={type}
      maxLength={maxLength}
      onKeyDown={onKeyDown}
      onChange={(e) => onChange(e.target.value)}
    />
  );
}

export default Input;
