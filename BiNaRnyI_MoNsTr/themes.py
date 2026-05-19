import typing

# Набір кольорових тем для інтерфейсу додатка
THEMES: typing.Dict[str, typing.Dict[str, str]] = {
    "cyberpunk": {
        "bg_main": "#0c0c0e", "bg_panel": "#141416", "btn_bg": "#232329",
        "btn_active": "#00e5ff", "entry_bg": "#232329", "border_color": "#3a3a42",
        "accent_green": "#00ff66", "accent_cyan": "#00e5ff", "accent_red": "#ff3b30",
        "fg_main": "#00e5ff", "text_fg": "#ffffff", "text_dim": "#8a8a93"
    },
    "matrix": {
        "bg_main": "#000000", "bg_panel": "#051505", "btn_bg": "#002200",
        "btn_active": "#33ff33", "entry_bg": "#001100", "border_color": "#005500",
        "accent_green": "#00ff00", "accent_cyan": "#33ff33", "accent_red": "#ff0000",
        "fg_main": "#00aa00", "text_fg": "#00cc00", "text_dim": "#008800"
    },
    "dracula": {
        "bg_main": "#282a36", "bg_panel": "#44475a", "btn_bg": "#6272a4",
        "btn_active": "#ff79c6", "entry_bg": "#282a36", "border_color": "#bd93f9",
        "accent_green": "#50fa7b", "accent_cyan": "#8be9fd", "accent_red": "#ff5555",
        "fg_main": "#bd93f9", "text_fg": "#f8f8f2", "text_dim": "#6272a4"
    },
    "nord": {
        "bg_main": "#2e3440", "bg_panel": "#3b4252", "btn_bg": "#434c5e",
        "btn_active": "#88c0d0", "entry_bg": "#2e3440", "border_color": "#4c566a",
        "accent_green": "#a3be8c", "accent_cyan": "#88c0d0", "accent_red": "#bf616a",
        "fg_main": "#88c0d0", "text_fg": "#d8dee9", "text_dim": "#4c566a"
    },
    "light": {
        "bg_main": "#f0f0f2", "bg_panel": "#ffffff", "btn_bg": "#e0e0e5",
        "btn_active": "#0088cc", "entry_bg": "#ffffff", "border_color": "#cccccc",
        "accent_green": "#00aa44", "accent_cyan": "#0088cc", "accent_red": "#cc0000",
        "fg_main": "#0088cc", "text_fg": "#000000", "text_dim": "#666666"
    },
    "one_dark": {
        "bg_main": "#282c34", "bg_panel": "#21252b", "btn_bg": "#3e4452",
        "btn_active": "#61afef", "entry_bg": "#1e222a", "border_color": "#5c6370",
        "accent_green": "#98c379", "accent_cyan": "#61afef", "accent_red": "#e06c75",
        "fg_main": "#61afef", "text_fg": "#abb2bf", "text_dim": "#5c6370"
    },
    "monokai": {
        "bg_main": "#272822", "bg_panel": "#1e1f1c", "btn_bg": "#3e3d32",
        "btn_active": "#66d9ef", "entry_bg": "#1e1f1c", "border_color": "#75715e",
        "accent_green": "#a6e22e", "accent_cyan": "#66d9ef", "accent_red": "#f92672",
        "fg_main": "#66d9ef", "text_fg": "#f8f8f2", "text_dim": "#75715e"
    },
    "solarized_dark": {
        "bg_main": "#002b36", "bg_panel": "#073642", "btn_bg": "#586e75",
        "btn_active": "#268bd2", "entry_bg": "#002b36", "border_color": "#93a1a1",
        "accent_green": "#859900", "accent_cyan": "#268bd2", "accent_red": "#dc322f",
        "fg_main": "#268bd2", "text_fg": "#93a1a1", "text_dim": "#586e75"
    },
    "gruvbox": {
        "bg_main": "#282828", "bg_panel": "#3c3836", "btn_bg": "#504945",
        "btn_active": "#83a598", "entry_bg": "#1d2021", "border_color": "#7c6f64",
        "accent_green": "#b8bb26", "accent_cyan": "#83a598", "accent_red": "#fb4934",
        "fg_main": "#83a598", "text_fg": "#ebdbb2", "text_dim": "#a89984"
    },
    "rose_pine": {
        "bg_main": "#191724", "bg_panel": "#1f1d2e", "btn_bg": "#26233a",
        "btn_active": "#c4a7e7", "entry_bg": "#1f1d2e", "border_color": "#403d52",
        "accent_green": "#9ccfd8", "accent_cyan": "#c4a7e7", "accent_red": "#ebbcba",
        "fg_main": "#c4a7e7", "text_fg": "#e0def4", "text_dim": "#908caa"
    }
}