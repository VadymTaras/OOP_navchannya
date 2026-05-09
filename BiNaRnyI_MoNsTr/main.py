from app_controller import AppController

def main():
    """Точка входу в БіНаРнОгО МоНсТрА."""
    monster = AppController()
    monster.start()

if __name__ == "__main__":
    try:
        main()
    except KeyboardInterrupt:
        # Гарний вихід при натисканні Ctrl+C
        print("\n\n [!] 'БіНаРниЙ МоНсТр' іде спати. Бувай!")