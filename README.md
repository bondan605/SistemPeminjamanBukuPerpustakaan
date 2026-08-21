# Sistem Peminjaman Buku Perpustakaan

Aplikasi berbasis web untuk mengelola sistem peminjaman buku di perpustakaan. Proyek ini memfasilitasi Admin untuk mengelola katalog buku serta persetujuan peminjaman, dan memungkinkan pengguna (Peminjam) untuk mengajukan peminjaman buku secara online.

## Persyaratan Sistem
Sebelum menjalankan proyek ini, pastikan Anda telah menginstal:
* **.NET SDK** (Sesuaikan dengan versi proyek Anda, misal: .NET 10)
* **SQL Server** (Atau disesuaikan dengan database provider yang digunakan)
* IDE seperti **Visual Studio 2022** atau **Visual Studio Code**

## Cara Instalasi dan Setup

### 1. Clone Repositori
Buka terminal atau Git Bash Anda, lalu jalankan perintah berikut untuk mengunduh source code:

    git clone https://github.com/bondan605/SistemPeminjamanBukuPerpustakaan.git
    cd SistemPeminjamanBukuPerpustakaan

### 2. Konfigurasi Database
Buka file appsettings.json (yang berada di dalam project Backend/API). Pastikan konfigurasi ConnectionStrings sudah disesuaikan dengan server database SQL lokal Anda. 

Contoh konfigurasi:

    "ConnectionStrings": {
      "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=DbPerpustakaan;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
    }

### 3. Migrasi Database (Penting!)
Proyek ini menggunakan Entity Framework Core. Anda harus melakukan migrasi untuk membuat tabel-tabel di database dan mengisi data awal (seeding). 

Buka terminal di folder project yang berisi DbContext, lalu jalankan perintah berikut:

    dotnet ef database update

> Catatan: Jika Anda menggunakan Visual Studio, Anda juga bisa membuka Package Manager Console (PMC) dan menjalankan perintah: Update-Database

### 4. Menjalankan Aplikasi
Setelah database berhasil dibuat, jalankan aplikasi melalui Visual Studio dengan menekan F5 (pastikan project Server/API di-set sebagai Startup Project), atau jalankan perintah ini di terminal:

    dotnet run

## Akun Demo / Default Credentials
Proyek ini sudah dilengkapi dengan data akun default (seeding) yang bisa langsung digunakan setelah migrasi database berhasil dilakukan. Silakan gunakan kredensial berikut untuk login:

### Kredensial Admin
Digunakan untuk mengelola katalog buku dan menyetujui/menolak pengajuan peminjaman.
* Email: admin@perpus.com
* Password: Admin@123

### Kredensial Peminjam (User)
Digunakan untuk melihat katalog dan mengajukan peminjaman buku.
* Email: john@gmail.com
* Password: John@123

---
⭐ Jangan lupa berikan star pada repositori ini jika proyek ini membantu Anda!
