import type { ReactNode } from "react";

type ButtonProps = {
  children: ReactNode;

  type?: "button" | "submit";

  onClick?: () => void;
};

function Button({ children, type = "button", onClick }: ButtonProps) {
  return (
    <button
      type={type}
      onClick={onClick}
      className="
        bg-blue-600
        text-white

        px-4
        py-2

        rounded

        cursor-pointer

        hover:bg-blue-700

        active:scale-95

        focus:outline-none
        focus:ring-2
        focus:ring-blue-400

        transition
        duration-200
      "
    >
      {children}
    </button>
  );
}

export default Button;
