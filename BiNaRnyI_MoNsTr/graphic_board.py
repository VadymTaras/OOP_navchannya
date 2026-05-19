import tkinter as tk
from themes import THEMES

class GraphicBoard:
    # Відображення бітової сітки розрядів на Canvas
    def __init__(self, master: tk.Frame, font_style) -> None:
        self.master = master
        self.font = font_style

        # Збереження останнього стану для зміни розмірів вікна
        self.last_data = None
        self.last_theme_key = None

        self.canvas = tk.Canvas(master, height=215, bd=0, highlightthickness=0)
        self.canvas.pack(fill=tk.BOTH, expand=True, padx=5, pady=5)

        # Подія зміни розмірів вікна
        self.canvas.bind("<Configure>", self._on_resize)

    def _on_resize(self, event) -> None:
        # Перемальовування сітки при зміні розміру екрана
        if self.last_data and self.last_theme_key:
            self.draw_canvas(self.last_data, self.last_theme_key)
        elif self.last_theme_key:
            self.draw_placeholder(self.last_theme_key)

    def draw_placeholder(self, theme_key: str) -> None:
        self.last_theme_key = theme_key
        self.canvas.delete("all")
        theme = THEMES[theme_key]

        self.canvas.config(bg=theme["bg_main"])

        canvas_width = self.canvas.winfo_width()
        if canvas_width <= 1:
            canvas_width = 670
        cx = canvas_width // 2

        self.canvas.create_text(
            cx, 107,
            text="[ Екран для відображення стовпчика обчислення бітів ]",
            font=self.font,
            fill=theme["text_dim"],
            justify=tk.CENTER
        )

    def draw_canvas(self, data: dict, theme_key: str) -> None:
        self.canvas.delete("all")
        theme = THEMES[theme_key]

        self.last_data = data
        self.last_theme_key = theme_key

        self.canvas.config(bg=theme["bg_panel"])

        carries = str(data.get("carries", "00000000")).zfill(8)[-8:]
        op_type = data.get("type", "+")
        s1 = str(data.get("s1", "00000000")).zfill(8)[-8:]
        s2 = str(data.get("s2", "00000000")).zfill(8)[-8:]
        res = str(data.get("res", "00000000")).zfill(8)[-8:]

        canvas_width = self.canvas.winfo_width()
        if canvas_width <= 1:
            canvas_width = 670

        cell_size = 40
        board_width = 8 * cell_size
        x_start = (canvas_width - board_width) // 2
        y_start = 40

        for i in range(8):
            if carries[i] == "1":
                self.canvas.create_text(
                    x_start + i * cell_size + 20, y_start - 15,
                    text="1", font=self.font, fill=theme["accent_red"]
                )

        for i in range(8):
            self.canvas.create_rectangle(
                x_start + i * cell_size, y_start,
                x_start + (i + 1) * cell_size, y_start + cell_size,
                outline=theme["btn_bg"], width=1
            )
            self.canvas.create_text(
                x_start + i * cell_size + 20, y_start + 20,
                text=s1[i], font=self.font, fill=theme["text_fg"]
            )

        self.canvas.create_text(
            x_start - 30, y_start + cell_size + 20,
            text=op_type, font=self.font, fill=theme["accent_green"]
        )

        for i in range(8):
            self.canvas.create_rectangle(
                x_start + i * cell_size, y_start + cell_size + 10,
                x_start + (i + 1) * cell_size, y_start + 2 * cell_size + 10,
                outline=theme["btn_bg"], width=1
            )
            self.canvas.create_text(
                x_start + i * cell_size + 20, y_start + cell_size + 30,
                text=s2[i], font=self.font, fill=theme["text_fg"]
            )

        self.canvas.create_line(
            x_start - 40, y_start + 2 * cell_size + 20,
            x_start + board_width + 10, y_start + 2 * cell_size + 20,
            fill=theme["text_fg"], width=2
        )

        for i in range(8):
            self.canvas.create_rectangle(
                x_start + i * cell_size, y_start + 2 * cell_size + 30,
                x_start + (i + 1) * cell_size, y_start + 3 * cell_size + 30,
                outline=theme["accent_green"], width=2
            )
            self.canvas.create_text(
                x_start + i * cell_size + 20, y_start + 2 * cell_size + 50,
                text=res[i], font=self.font, fill=theme["accent_green"]
            )