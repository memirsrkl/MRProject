TOPLANTI ODASI REZERVASYON SİSTEMİ

Bu proje, .NET Developer teknik değerlendirmesi kapsamında geliştirilmiş bir Toplantı Odası Rezervasyon Sistemidir.
Sistem; kullanıcıların toplantı odası rezervasyonu oluşturmasını, katılımcı yönetmesini ve yetkili (Admin) tarafından rezervasyonların onaylanmasını sağlayan web tabanlı bir uygulamadır.

KULLANILAN TEKNOLOJİLER

ASP.NET Core MVC
Entity Framework Core
SQLite
MediatR (CQRS Pattern)
Razor View Engine
Cookie Authentication (Role-based Authorization)
Bootstrap 5
FullCalendar
SweetAlert2
Git

MİMARİ YAPI

Proje katmanlı mimari prensiplerine uygun olarak Onion Architecture prensiplerine göre geliştirilmiştir.

MRP.Web → Presentation Layer (MVC, Controllers, Views)
Application → Business Logic (CQRS, MediatR, Commands & Queries)
Domain → Entities, Enums, Extensions
Persistence → DbContext, Database Configurations

MİMARİ TERCİHLER

CQRS yaklaşımı (Command & Query ayrımı)
MediatR ile iş katmanı izolasyonu
Backend seviyesinde iş kurallarının enforce edilmesi (Frontend’e güvenilmemiştir)
Role-based Authorization
Tek noktadan SaveChanges kullanılarak atomiklik sağlanması
Katmanlı mimari ile sorumluluk ayrımı

ROL MODELİ

Sistemde iki rol bulunmaktadır.

User
Rezervasyon oluşturabilir.
Katılımcı ekleyebilir / çıkarabilir (sadece onaylı ve gelecekteki rezervasyonlarda).
Sahibi olduğu rezervasyonları iptal edebilir.
Katılımcısı olduğu rezervasyonları görüntüleyebilir.

Admin
Tüm rezervasyonları görüntüleyebilir.
Rezervasyonları Onaylayabilir / Reddedebilir.
Reddederken açıklama girebilir.
Yeni toplantı odası ekleyebilir.

TOPLANTI ODALARI

Sistem başlangıçta toplantı odaları ile çalışacak şekilde tasarlanmıştır.
Admin paneli üzerinden yeni toplantı odaları eklenebilir.

Her oda için aşağıdaki bilgiler tutulmaktadır:
Açıklama
Lokasyon
Kapasite

REZERVASYON KURALLARI

Tarih Kısıtı

Geçmiş tarihli rezervasyon oluşturulamaz.
Rezervasyonlar yalnızca içinde bulunulan takvim haftası (Pazartesi–Pazar) ve onu takip eden takvim haftası için oluşturulabilir.

Bu kontrol backend seviyesinde uygulanmaktadır.

Oda Çakışma Kuralı

Aynı toplantı odası için:
Aynı tarih ve çakışan saat aralığında rezervasyon oluşturulamaz.

Bu kontrol Onay Bekleyen (Pending) rezervasyonlar dahil olmak üzere yapılmaktadır.

Katılımcı Çakışma Kuralı

Bir kullanıcı aynı tarih ve çakışan saat aralığında birden fazla toplantıya katılamaz.

Rezervasyon Durumu

Rezervasyon oluşturulduğunda varsayılan durum:
Pending (Onay Bekliyor)

Admin tarafından:
Approved (Onaylandı)
Rejected (Reddedildi – opsiyonel açıklama ile)
olarak güncellenebilir.

REZERVASYON GÜNCELLEME KURALLARI

Sadece onaylanmış ve gelecekteki rezervasyonlar düzenlenebilir.

Düzenlenebilir alanlar:
Toplantı adı
Katılımcı listesi

Değiştirilemez alanlar:
Tarih
Saat

REZERVASYON SİLME (İPTAL)

Kullanıcı yalnızca toplantı sahibi olduğu rezervasyonu iptal edebilir.
Geçmiş rezervasyonlar iptal edilemez.
Yetki kontrolleri backend seviyesinde yapılmaktadır.

KİMLİK DOĞRULAMA

Cookie Authentication kullanılmaktadır.

Admin kullanıcısı bilgileri atlanmış bu sebeple ekleme yapıyorum.
kullanıcı adı: admin
şifre: 154273154273
Claim tabanlı rol yönetimi uygulanmıştır.
Basit kullanıcı kayıt ve giriş sistemi bulunmaktadır.
