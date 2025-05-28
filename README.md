# Vector Arama ve SQL Server 2025 Yenilikleri

Vector arama, verilerin çok boyutlu sayısal vektörler olarak temsil edilip, bu vektörler arasında benzerlik (similarity) veya mesafe (distance) bazlı arama yapılmasıdır. Özellikle yapay zeka, doğal dil işleme (NLP), görsel arama ve öneri sistemlerinde kullanılır.

- **Veri Temsili:** Metin, resim, video gibi karmaşık veriler, embedding (gömme) modelleri ile sayısal vektörlere dönüştürülür.
- **Benzerlik Ölçümü:** En yaygın metrikler arasında *Cosine Similarity*, *Euclidean Distance* ve *Manhattan Distance* yer alır.
- **Amaç:** Arama sorgusuna en yakın (benzer) vektörleri hızlıca bulmak.

---

## SQL Server 2025'te Vector Arama Yenilikleri

SQL Server 2025, yapay zeka ve makine öğrenimi uygulamalarını daha güçlü desteklemek için **yerleşik vector arama özellikleri** getirdi. İşte öne çıkanlar:

### 1. Native Vector Veri Tipi

- `VECTOR` veri tipi eklendi. Çok boyutlu vektörler doğrudan tablo sütunlarında saklanabilir.
- Performans ve depolama için optimize edilmiştir.

### 2. Vector İndeksleme (ANN - Approximate Nearest Neighbor)

- Milyonlarca vektör arasında hızlı benzerlik araması için **Approximate Nearest Neighbor (ANN)** indeksleri destekleniyor.
- Bu sayede saniyeler içinde en yakın sonuçlar bulunabiliyor.

### 3. Yerleşik Benzerlik Fonksiyonları

- `COSINE_SIMILARITY(vector1, vector2)`
- `EUCLIDEAN_DISTANCE(vector1, vector2)`
- `DOT_PRODUCT(vector1, vector2)`

Bu fonksiyonlar doğrudan SQL sorgularında kullanılabilir.

### 4. AI Entegrasyon Kolaylığı

- .NET, Python gibi dillerle üretilen embedding vektörler kolayca SQL Server’a yazılabilir.
- Böylece yapay zeka destekli arama ve öneri sistemleri yüksek performansla veritabanı içinde çalıştırılabilir.

---

## Örnek SQL Kullanımı

```sql
-- Tablo oluşturma örneği
CREATE TABLE [dbo].[Products](
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [Name] NVARCHAR(100),
    [Description] NVARCHAR(300),
    [Price] DECIMAL(18, 2),
    [Category] NVARCHAR(100),
    [EmbeddingVector] VECTOR(768) -- 768 boyutlu embedding vektörü
);

-- Örnek: En yakın 5 ürünü arama
DECLARE @queryVector VECTOR(768) = ... -- Sorgu embedding vektörü burada atanmalı

SELECT TOP 5 
    Id, 
    Name, 
    Description, 
    COSINE_SIMILARITY(EmbeddingVector, @queryVector) AS SimilarityScore
FROM Products
ORDER BY SimilarityScore DESC;
```
## Microsoft Örneği 
```sql
DECLARE @v1 VECTOR(2) = '[1,1]';
DECLARE @v2 VECTOR(2) = '[-1,-1]';

SELECT 
    VECTOR_DISTANCE('euclidean', @v1, @v2) AS euclidean,
    VECTOR_DISTANCE('cosine', @v1, @v2) AS cosine,
    VECTOR_DISTANCE('dot', @v1, @v2) AS negative_dot_product;
```


## Projeye ait ekran görüntüleri : 

![SimilarityPng2](https://github.com/user-attachments/assets/4e9316ab-f7a8-407e-b7ed-3716e286f7d3)

![SimilarityPng](https://github.com/user-attachments/assets/8d9286bf-2330-4065-b9a3-9c114eb41bd4)


# Kullanım Rehberi
--- 
## 1. Semantic Kernel ve Ollama Connector Kurulumu

-  [Semantic Kernel](https://aka.ms/semantic-kernel) kütüphanesini projenize dahil edin:
  
  ```bash
  dotnet add package Microsoft.SemanticKernel
  ```
  ```bash
     dotnet add package Microsoft.SemanticKernel.Connectors.Ollama --version 1.35.0-alpha
  ```
## 2.  Entity Framework Core kütüphanelerini Kurulumu
 ```bash
    dotnet add package Microsoft.EntityFrameworkCore
    dotnet add package Microsoft.EntityFrameworkCore.SqlServer
    dotnet add package Microsoft.EntityFrameworkCore.Tools
    dotnet add package EFCore.SqlServer.VectorSearch 
```

