# Multiple Copy Paste Manager

Program multiple copy-paste dengan GUI Windows Forms dan database SQLite yang dibuat oleh Syaiful Wachid.

**🔥 Hotkeys:** F1=Copy, F2=Paste, F4=Search, F5=Statistics, F6=Settings, F7=About

**👨‍💻 Developer:** Syaiful Wachid - Senior Project Designer at Fiberhome Indonesia

---

**🎯 Quick Start:** Press F1 to copy, F2 to paste, F4 to search, F5 for stats, F6 for settings, F7 for about!

## Fitur Utama

### 🎯 Core Features
- **🔥 F1**: Copy teks yang dipilih ke dalam list
- **🔥 F2**: Paste item berikutnya dari list (FIFO - First In First Out)
- **🔍 F4**: Search items dengan interface pencarian
- **📊 F5**: View statistics dan analisis penggunaan
- **⚙️ F6**: Open settings dan konfigurasi
- **ℹ️ F7**: Show about information
- **💾 Database SQLite**: Menyimpan semua item yang di-copy secara permanen
- **🖥️ System Tray**: Berjalan di background dengan icon di notification tray

### 🖥️ GUI Features
- **🎨 Modern Interface**: Windows Forms dengan desain yang clean dan user-friendly
- **📋 Real-time List**: Menampilkan semua item yang sudah di-copy
- **🖱️ Double-click**: Double-click item untuk copy ke clipboard
- **🔘 Button Controls**: Tombol untuk copy, paste, clear, minimize, export, import, search, statistics, settings, dan about
- **🪟 Multiple Forms**: Search, Statistics, Settings, dan About windows
- **📱 Context Menu**: Right-click tray icon untuk akses cepat ke semua fitur

### 🔧 Advanced Features
- **⌨️ Advanced Hotkey Support**: F1-F7 berfungsi di seluruh sistem
- **📱 Context Menu**: Right-click tray icon untuk menu lengkap dengan hotkeys
- **🔔 Notifications**: Balloon tips untuk feedback user
- **📋 Clipboard Preservation**: Mempertahankan clipboard original
- **⬇️ Auto-minimize**: Otomatis minimize ke tray saat close
- **📤 Export/Import**: Backup dan restore data dalam format JSON
- **🔍 Search Function**: Pencarian item berdasarkan konten dengan interface terpisah (F4)
- **📊 Statistics**: Analisis penggunaan dan statistik detail dengan grafik (F5)
- **⚙️ Settings**: Konfigurasi aplikasi yang dapat disesuaikan (F6)
- **ℹ️ About**: Informasi developer dan fitur aplikasi (F7)
- **🚀 Auto-startup**: Opsi untuk menjalankan aplikasi saat Windows startup
- **💾 Auto-backup**: Backup otomatis database dengan rotasi file
- **💾 Data Persistence**: Data tersimpan permanen di SQLite database
- **🎨 Modern UI**: Interface yang clean dan user-friendly dengan multiple forms

## Cara Penggunaan

### 1. Install dan Build
```bash
# Clone atau download project
cd MultipleCopyPasteCSharp

# Restore packages
dotnet restore

# Build project
dotnet build

# Run aplikasi (atau double-click build_and_run.bat)
dotnet run
```

### 2. Penggunaan Dasar
1. **🔥 Copy Text**: Pilih teks di aplikasi manapun, tekan **F1**
2. **🔥 Paste Text**: Tekan **F2** untuk paste item berikutnya
3. **🔍 Search Items**: Tekan **F4** atau klik tombol "Search Items" untuk mencari item tertentu
4. **📊 View Statistics**: Tekan **F5** atau klik "Statistics" untuk melihat analisis penggunaan
5. **⚙️ Settings**: Tekan **F6** atau klik "Settings" untuk mengkonfigurasi aplikasi
6. **ℹ️ About**: Tekan **F7** atau klik "About" untuk informasi developer dan fitur
7. **📋 View Items**: Buka GUI untuk melihat semua item yang tersimpan
8. **🔧 Manage Items**: Gunakan tombol di GUI untuk mengelola item
9. **📤 Export/Import**: Backup data dengan "Export Data" atau restore dengan "Import Data"

### 3. System Tray
- **🖱️ Left-click**: Buka GUI utama
- **🖱️ Right-click**: Menu konteks dengan opsi:
  - Show Main Window
  - Copy Selected (F1)
  - Paste Next (F2)
  - Search Items (F4)
  - Statistics (F5)
  - Settings (F6)
  - About (F7)
  - Create Backup
  - Clear All Items
  - Exit

### 4. Keyboard Shortcuts Summary
| Key | Function | Description |
|-----|----------|-------------|
| **🔥 F1** | Copy | Copy selected text to clipboard queue |
| **🔥 F2** | Paste | Paste next item from clipboard queue |
| **🔍 F4** | Search | Open search window for finding items |
| **📊 F5** | Statistics | Open statistics window for usage analysis |
| **⚙️ F6** | Settings | Open settings window for configuration |
| **ℹ️ F7** | About | Open about window with developer info |

## Keyboard Shortcuts

| Key | Function | Description |
|-----|----------|-------------|
| **🔥 F1** | Copy | Copy selected text to clipboard queue |
| **🔥 F2** | Paste | Paste next item from clipboard queue |
| **🔍 F4** | Search | Open search window for finding items |
| **📊 F5** | Statistics | Open statistics window for usage analysis |
| **⚙️ F6** | Settings | Open settings window for configuration |
| **ℹ️ F7** | About | Open about window with developer info |

**💡 Note:** All hotkeys work globally across all applications when the program is running.

## Struktur Database

Database SQLite (`copied_items.db`) memiliki struktur:
```sql
CREATE TABLE copied_items (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    content TEXT NOT NULL,
    timestamp DATETIME DEFAULT CURRENT_TIMESTAMP
);
```

**💾 Database Features:**
- Automatic timestamp tracking
- Content indexing for fast search
- Backup and restore functionality
- Export to JSON format

## Fitur Tambahan

### 🔄 Persistence
- **💾 Data tersimpan di database SQLite**
- **🔄 Item tidak hilang saat restart aplikasi**
- **⏰ Timestamp untuk tracking waktu copy**
- **💾 Auto-backup dengan rotasi file**
- **📤 Export/Import dalam format JSON**
- **⚙️ Settings tersimpan di file konfigurasi**
- **🚀 Startup registry integration**
- **⌨️ Hotkey preferences saved**
- **📋 Backup history maintained**

### 🎨 UI/UX
- **📱 Responsive design**
- **🎨 Color-coded elements**
- **🔘 Intuitive button layout**
- **✨ Professional appearance**
- **🪟 Multiple form windows**
- **📱 Context menu integration**
- **⌨️ Advanced keyboard shortcut support (F1-F7)**
- **🖥️ Modern Windows Forms interface**
- **⌨️ Global hotkey indicators**
- **🖥️ System tray integration**

### ⚡ Performance
- **⚡ Lightweight application**
- **⌨️ Fast hotkey response (F1-F7)**
- **💾 Efficient database operations**
- **💾 Minimal memory usage**
- **💾 Optimized backup system**
- **🔍 Quick search functionality**
- **📱 Responsive UI design**
- **⌨️ Global hotkey registration**
- **🖥️ System tray integration**

## Technical Details

### Dependencies
- **🖥️ .NET 6.0 Windows**
- **💾 System.Data.SQLite.Core**
- **🖥️ Windows Forms**
- **🔧 Microsoft.Win32 (Registry access)**
- **📄 System.Text.Json (JSON serialization)**
- **⚡ System.Threading.Tasks (Async operations)**
- **⌨️ System.Runtime.InteropServices (Hotkey registration)**

### Architecture
- **⚡ Single-threaded with async operations**
- **🔄 Event-driven programming**
- **💾 Database-first approach**
- **🏗️ Modular code structure**
- **⌨️ Advanced hotkey management system (F1-F7)**
- **💾 Backup and restore functionality**
- **⚙️ Settings persistence**
- **🪟 Multi-form application design**
- **⌨️ Global hotkey registration**
- **🖥️ System tray integration**

## Developer Information

**👨‍💻 Dibuat oleh:** Syaiful Wachid  
**🏢 Senior Project Designer:** Fiberhome Indonesia  
**🔗 LinkedIn Profile:** https://www.linkedin.com/in/syaiful-wachid-5373n/

**🚀 Features Developed:**
- **⌨️ Advanced hotkey system (F1-F7)**
- **🪟 Multiple form windows**
- **💾 Database integration**
- **🖥️ System tray functionality**
- **💾 Backup and restore system**
- **⚙️ Settings management**
- **📊 Statistics and analytics**

## License

This project is created for educational and personal use.

**© 2024 Syaiful Wachid - Fiberhome Indonesia**

## Changelog

### Version 1.0.0 (Latest)
- **🚀 Initial release with advanced features**
- **⌨️ Advanced hotkey system (F1-F7)** for all major functions
- **🪟 Multiple form windows** for better user experience
- **💾 SQLite database integration** for persistent storage
- **🖥️ System tray support** with comprehensive context menu
- **🎨 Modern GUI interface** with professional design
- **📤 Export/Import functionality** in JSON format
- **🔍 Search and statistics features** with dedicated windows
- **⚙️ Settings and configuration options** with persistence
- **🚀 Auto-startup and auto-backup features** for convenience
- **ℹ️ About window** with developer information
- **⌨️ Global hotkey support** across all applications
- **💾 Backup and restore functionality** with file rotation
- **📊 Usage analytics and statistics** for insights
- **👨‍💻 Professional branding** with developer information 