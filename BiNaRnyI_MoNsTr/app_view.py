import tkinter as tk
from tkinter import font
from tkinter import messagebox
from themes import THEMES
from graphic_board import GraphicBoard
import demo_manager

class AppView:
    # Клас для відображення вікна додатка зі спрощеними шкільними підказками
    def __init__(self, root: tk.Tk) -> None:
        self.root = root
        self.root.title("БіНаРнИй МоНсТр 3.0 - MVC Edition")
        self.root.geometry("700x700")
        self.root.minsize(650, 600)

        self.themes = THEMES
        self.current_theme_key = "cyberpunk"

        self.font_main = font.Font(family="Courier New", size=11)
        self.font_logo = font.Font(family="Courier New", size=13)
        self.font_digits = font.Font(family="Courier New", size=12)

        self.header_btns = {}
        self.op_buttons = {}

        self._build_ui()

    def _build_ui(self) -> None:
        self.header_frame = tk.Frame(self.root)
        self.header_frame.pack(side=tk.TOP, fill=tk.X, padx=15, pady=10)

        self.header_frame.grid_columnconfigure(2, weight=1)

        self.mode_label = tk.Label(self.header_frame, text="РЕЖИМ: BINARY", font=self.font_main, width=16, anchor=tk.W)
        self.mode_label.grid(row=0, column=0, padx=5, pady=5, sticky=tk.W)

        btn_mode = tk.Button(self.header_frame, text="⇄ Змінити режим", font=self.font_main, bd=1, relief=tk.SOLID, padx=5)
        btn_mode.grid(row=0, column=1, padx=5, pady=5, sticky=tk.W)
        self.header_btns["toggle_mode"] = btn_mode
        self._bind_tooltip(btn_mode, "Перемикання режиму: двійкові або звичайні числа")

        btn_demo = tk.Button(
            self.header_frame,
            text="🚀 Демо",
            font=self.font_main,
            bd=1,
            relief=tk.SOLID,
            padx=5,
            command=lambda: demo_manager.run_demo(
                self.entry_a,
                self.entry_b,
                self.mode_label.cget("text"),
                self.op_buttons
            )
        )
        btn_demo.grid(row=0, column=2, padx=5, pady=5, sticky=tk.W)
        self.header_btns["demo"] = btn_demo
        self._bind_tooltip(btn_demo, "Заповнити поля випадковими числами")

        btn_theme = tk.Button(self.header_frame, text="🎨 Тема", font=self.font_main, bd=1, relief=tk.SOLID, padx=5)
        btn_theme.grid(row=0, column=3, padx=5, pady=5, sticky=tk.E)
        self.header_btns["toggle_theme"] = btn_theme
        self._bind_tooltip(btn_theme, "Змінити колір оформлення програми")

        btn_hist = tk.Button(self.header_frame, text="💾 Історія", font=self.font_main, bd=1, relief=tk.SOLID, padx=5)
        btn_hist.grid(row=1, column=0, padx=5, pady=5, sticky=tk.W)
        self.header_btns["show_history"] = btn_hist
        self._bind_tooltip(btn_hist, "Показати історію попередніх обчислень")

        btn_clear = tk.Button(self.header_frame, text="🗑️ Очистити", font=self.font_main, bd=1, relief=tk.SOLID, padx=5)
        btn_clear.grid(row=1, column=1, padx=5, pady=5, sticky=tk.W)
        self.header_btns["clear_history"] = btn_clear
        self._bind_tooltip(btn_clear, "Стерти всю історію обчислень")

        self.input_frame = tk.Frame(self.root)
        self.input_frame.pack(side=tk.TOP, fill=tk.X, padx=15, pady=5)

        tk.Label(self.input_frame, text="Число A:", font=self.font_main).grid(row=0, column=0, padx=5, pady=5, sticky=tk.W)
        self.entry_a = tk.Entry(self.input_frame, font=self.font_digits, justify=tk.LEFT, bd=0, highlightthickness=1)
        self.entry_a.grid(row=0, column=1, columnspan=4, padx=5, pady=5, sticky=tk.EW)

        tk.Label(self.input_frame, text="Число B:", font=self.font_main).grid(row=1, column=0, padx=5, pady=5, sticky=tk.W)
        self.entry_b = tk.Entry(self.input_frame, font=self.font_digits, justify=tk.LEFT, bd=0, highlightthickness=1)
        self.entry_b.grid(row=1, column=1, columnspan=4, padx=5, pady=5, sticky=tk.EW)

        self.input_frame.grid_columnconfigure(1, weight=1)

        self.ops_frame = tk.Frame(self.root)
        self.ops_frame.pack(side=tk.TOP, fill=tk.X, padx=15, pady=10)

        operations = [
            ["+", "-", "*", "/", "&"],
            ["|", "^", "~", "<<", ">>"]
        ]

        op_hints = {
            "+": "Звичайне додавання двох чисел",
            "-": "Віднімання другого числа від першого",
            "*": "Множення чисел між собою",
            "/": "Ділення націло з отриманням остачі",
            "&": "Залишає 1, тільки якщо в обох числах стоять 1",
            "|": "Ставить 1, якщо хоча б в одному числі є 1",
            "^": "Ставить 1, якщо біти в числах різні",
            "~": "Змінює кожен біт числа A на протилежний",
            "<<": "Зсуває біти числа A вліво на кількість місць з числа B",
            ">>": "Зсуває біти числа A вправо на кількість місць з числа B"
        }

        for r_idx, row in enumerate(operations):
            for c_idx, op in enumerate(row):
                btn = tk.Button(self.ops_frame, text=op, font=self.font_digits, width=6, height=1, bd=1, relief=tk.SOLID)
                btn.grid(row=r_idx, column=c_idx, padx=5, pady=5, sticky=tk.NSEW)
                self.op_buttons[op] = btn
                self.ops_frame.grid_columnconfigure(c_idx, weight=1)
                self._bind_tooltip(btn, op_hints.get(op, ""))

        self.res_frame = tk.Frame(self.root, highlightthickness=1)
        self.res_frame.pack(side=tk.TOP, fill=tk.X, padx=15, pady=5)

        self.res_label = tk.Label(self.res_frame, text="РЕЗУЛЬТАТ:\nОчікування введення чисел", font=self.font_logo, justify=tk.CENTER, height=4, pady=5)
        self.res_label.pack(fill=tk.X)

        self.board_frame = tk.Frame(self.root, highlightthickness=1)
        self.board_frame.pack(side=tk.TOP, fill=tk.X, padx=15, pady=5)

        self.board = GraphicBoard(self.board_frame, self.font_digits)

        self.log_frame = tk.Frame(self.root)
        self.log_frame.pack(side=tk.TOP, fill=tk.BOTH, expand=True, padx=15, pady=(5, 20))

        self.log_header = tk.Frame(self.log_frame)
        self.log_header.pack(side=tk.TOP, fill=tk.X, pady=2)

        # Повертаю гарну українську назву для текстового фрейму
        self.log_title = tk.Label(self.log_header, text="Покроковий журнал дій", font=self.font_main)
        self.log_title.pack(side=tk.LEFT, anchor=tk.W)

        btn_save_log = tk.Button(self.log_header, text="💾 Зберегти", font=self.font_main, bd=1, relief=tk.SOLID, padx=5, command=self._save_log_to_file)
        btn_save_log.pack(side=tk.RIGHT, anchor=tk.E)
        self.header_btns["save_log"] = btn_save_log
        self._bind_tooltip(btn_save_log, "Зберегти текст обчислень у файл protocol.txt")

        self.scrollbar = tk.Scrollbar(self.log_frame)
        self.scrollbar.pack(side=tk.RIGHT, fill=tk.Y)

        self.steps_area = tk.Text(self.log_frame, font=self.font_main, yscrollcommand=self.scrollbar.set, bd=1, wrap=tk.WORD, relief=tk.SOLID)
        self.steps_area.pack(side=tk.LEFT, fill=tk.BOTH, expand=True)
        self.scrollbar.config(command=self.steps_area.yview)

        self.steps_area.bind("<Key>", lambda e: "break")

    def _bind_tooltip(self, widget: tk.Widget, text: str) -> None:
        tooltip_window = None

        def show_tooltip(event) -> None:
            nonlocal tooltip_window
            if tooltip_window or not text:
                return

            tooltip_window = tk.Toplevel(widget)
            tooltip_window.wm_overrideredirect(True)

            theme = self.themes[self.current_theme_key]
            label = tk.Label(
                tooltip_window,
                text=text,
                justify=tk.LEFT,
                background=theme["entry_bg"],
                foreground=theme["accent_cyan"],
                relief=tk.SOLID,
                borderwidth=1,
                font=self.font_main,
                padx=6,
                pady=4
            )
            label.pack()

            tooltip_window.update_idletasks()
            tw = tooltip_window.winfo_reqwidth()
            th = tooltip_window.winfo_reqheight()

            x = event.x_root - (tw // 2)
            y = event.y_root - th - 8

            screen_width = widget.winfo_screenwidth()
            if x + tw > screen_width:
                x = screen_width - tw - 10

            if x < 10:
                x = 10

            if y < 10:
                y = event.y_root + 18

            tooltip_window.wm_geometry(f"+{x}+{y}")

        def hide_tooltip(event) -> None:
            nonlocal tooltip_window
            if tooltip_window:
                tooltip_window.destroy()
                tooltip_window = None

        widget.bind("<Enter>", show_tooltip)
        widget.bind("<Leave>", hide_tooltip)

    def _save_log_to_file(self) -> None:
        text_content = self.steps_area.get("1.0", tk.END).strip()
        try:
            with open("protocol.txt", "w", encoding="utf-8") as f:
                f.write(text_content)
            messagebox.showinfo("Успіх", "Обчислення успішно збережено у файл protocol.txt")
        except Exception as e:
            messagebox.showerror("Помилка", f"Не вдалося зберегти файл: {str(e)}")

    def update_colors(self, theme_key: str, active_op: str = None) -> None:
        self.current_theme_key = theme_key
        theme = self.themes[theme_key]

        self.root.config(bg=theme["bg_main"])
        self.header_frame.config(bg=theme["bg_main"])
        self.input_frame.config(bg=theme["bg_main"])
        self.ops_frame.config(bg=theme["bg_main"])
        self.log_frame.config(bg=theme["bg_main"])
        self.log_header.config(bg=theme["bg_main"])

        self.mode_label.config(bg=theme["bg_main"], fg=theme["accent_green"])
        self.log_title.config(bg=theme["bg_main"], fg=theme["text_dim"])

        self.res_frame.config(bg=theme["bg_main"], highlightbackground=theme["accent_cyan"])
        self.res_label.config(bg=theme["bg_main"], fg=theme["accent_cyan"])

        self.board_frame.config(bg=theme["bg_main"], highlightbackground=theme["accent_green"])

        for child in self.input_frame.winfo_children():
            if isinstance(child, tk.Label):
                child.config(bg=theme["bg_main"], fg=theme["text_dim"])

        for entry in [self.entry_a, self.entry_b]:
            entry.config(
                bg=theme["entry_bg"],
                fg=theme["text_fg"],
                insertbackground=theme["fg_main"],
                highlightbackground=theme["border_color"],
                highlightcolor=theme["fg_main"]
            )

        for key, btn in self.header_btns.items():
            btn.config(bg=theme["btn_bg"], fg=theme["text_fg"], activebackground=theme["btn_active"], activeforeground=theme["bg_main"])
            if key == "clear_history":
                btn.config(fg=theme["accent_red"])
            elif key in ["show_history", "save_log"]:
                btn.config(fg=theme["accent_cyan"])
            elif key == "demo":
                btn.config(fg=theme["accent_green"])

        for op_key, btn in self.op_buttons.items():
            if op_key == active_op:
                btn.config(bg=theme["btn_active"], fg=theme["bg_main"], activebackground=theme["btn_active"], relief=tk.SUNKEN)
            else:
                btn.config(bg=theme["btn_bg"], fg=theme["accent_green"], activebackground=theme["btn_active"], activeforeground=theme["bg_main"], relief=tk.SOLID)

        self.steps_area.config(bg=theme["entry_bg"], fg=theme["text_fg"], insertbackground=theme["fg_main"])
        self.scrollbar.config(bg=theme["bg_main"])

        self.steps_area.tag_config("info", foreground=theme["fg_main"])
        self.steps_area.tag_config("result", foreground=theme["accent_green"])
        self.steps_area.tag_config("error", foreground=theme["accent_red"])