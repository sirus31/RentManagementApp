type ModalProps = {
  title: string;

  children: React.ReactNode;

  onClose: () => void;
};

function Modal({ title, children, onClose }: ModalProps) {
  return (
    <div
      className="
      fixed
      inset-0
      bg-black/40

      flex
      items-center
      justify-center
      "
    >
      <div
        className="
        bg-white

        rounded-lg

        p-6

        w-[400px]

        shadow-lg
        "
      >
        <div
          className="
          flex
          justify-between
          mb-5
          "
        >
          <h2 className="text-xl font-bold">{title}</h2>

          <button onClick={onClose} className="text-xl">
            ×
          </button>
        </div>

        {children}
      </div>
    </div>
  );
}

export default Modal;
