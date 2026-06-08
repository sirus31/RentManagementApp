type Option = {
  label: string;

  value: string | number;
};

type SelectProps = {
  value: string | number;

  options: Option[];

  onChange: (value: string) => void;
};

function Select({ value, options, onChange }: SelectProps) {
  return (
    <select
      className="
        border
        border-gray-300

        rounded-md

        px-3
        py-2

        bg-white

        outline-none

        focus:border-blue-500
        focus:ring-1
        focus:ring-blue-500

        transition
        duration-200

        min-w-[150px]
      "
      value={value}
      onChange={(e) => onChange(e.target.value)}
    >
      {options.map((option) => (
        <option key={option.value} value={option.value}>
          {option.label}
        </option>
      ))}
    </select>
  );
}

export default Select;
