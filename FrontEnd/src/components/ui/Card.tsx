import type { ReactNode } from "react";

type CardProps = {
  children: ReactNode;

  onClick?: () => void;
};

function Card({
  children,

  onClick,
}: CardProps) {
  return (
    <div
      onClick={onClick}
      className="
        bg-white

        shadow

        rounded-lg

        p-5


        cursor-pointer


        hover:shadow-lg

        hover:-translate-y-1


        active:scale-95


        transition

        duration-200
      "
    >
      {children}
    </div>
  );
}

export default Card;
