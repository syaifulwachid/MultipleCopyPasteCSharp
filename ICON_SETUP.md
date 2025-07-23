# Setup Icon untuk Multiple Copy Paste Manager

## Perubahan yang Telah Dilakukan

### 1. File Project (.csproj)
- Mengaktifkan `<ApplicationIcon>app.ico</ApplicationIcon>` untuk icon pada file EXE
- Menambahkan konfigurasi untuk menyalin file `app.ico` ke output directory

### 2. MainForm.cs
- Mengubah `this.Icon = SystemIcons.Application` menjadi `this.Icon = new Icon("app.ico")` untuk header window
- Mengubah `notifyIcon.Icon = SystemIcons.Application` menjadi `notifyIcon.Icon = new Icon("app.ico")` untuk tray icon

### 3. File Icon
- Membuat file `app.ico` dengan format yang valid menggunakan PowerShell
- Icon menampilkan simbol copy/paste dengan teks "CP" di tengah
- Background putih dengan elemen berwarna biru gelap

## Lokasi Icon yang Diterapkan

1. **File EXE**: Icon akan muncul di file `MultipleCopyPaste.exe` hasil build
2. **Header Window**: Icon akan muncul di pojok kiri atas window aplikasi
3. **Tray Icon**: Icon akan muncul di system tray saat aplikasi diminimize

## Cara Build dan Test

1. Jalankan `build_and_run.bat` untuk build dan run aplikasi
2. Atau gunakan command: `dotnet build --configuration Release`
3. Jalankan aplikasi dari folder `bin\Release\net6.0-windows\`

## Catatan

- Icon dibuat dengan ukuran 32x32 pixel
- Format ICO yang valid untuk kompatibilitas Windows
- Icon akan otomatis disalin ke output directory saat build 