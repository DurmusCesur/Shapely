# Shapely

Shapely is a great Python library that helps us analyse and manipulate geometries. We think it is important to include this library in Dynamo. 

Unlike dynamo, shapely uses its own geometry language. Therefore, transformations must be made between each other without losing data. In this package you will find a dynamo version of shapely geometry. Enjoy the polygons.


ShapelytoDynamo :
Bir dynamo geometrisini "Shapely" geometrisinine dönüştürür.
![ShapelytoDynamo](https://user-images.githubusercontent.com/24898802/229380656-36acf759-17d4-4cf0-bc87-f3436fd37d56.jpg)

Benzer bir yol ile elde edilen shapely geometrisinin yine dynamo geometrisine dönüşümüde mevcuttur.

DynamotoShapely :
Bir shapely geometrisinin hiçbir veri kaybı olmadan dynamo geometrisine dönüşütürür.
![DynamotoShapely](https://user-images.githubusercontent.com/24898802/229380730-eea47661-662a-48d4-8726-74ce6e0a87a8.jpg)

ShapelyArea :
Bir kapalı geometrinin (Polygon,polycurve) yüzey alanını hesaplamamıza izin verir.
![area](https://user-images.githubusercontent.com/24898802/229380757-fcbcb89e-18de-4698-b7e1-b65bf68c9423.jpg)

ShapelyBoundingBox :
Bir geometrinin boundingbox'ını bir geometri olarak döndürür. Sonuç herzaman bir polgon,polycurve ve o geometriyi oluşturan noktaların bir listesidir.
![boundıngbox](https://user-images.githubusercontent.com/24898802/229380814-0c31109f-5bed-45b9-8b74-d5e7db3cecd4.jpg)

ShapelyContains :
Bir geometrinin, noktalar ile ilişkisini analiz etmememize izin verir. Eğer noktalar geometri içerisindeyse "True" ,değilse "False" döndürür.
![contaıns](https://user-images.githubusercontent.com/24898802/229380863-f0166b06-a06f-48f2-b882-765616a5e5a7.gif)

ShapelyConvexHull :
Dynamo noktalarını kullanarak noktaların oluşturacağı en dış polygonu oluşturur. Sonuç her zaman bir polygon,polycurve ve o geometriyi oluşturan noktaların listesidir.
![convexhull](https://user-images.githubusercontent.com/24898802/229380997-f8d15c9b-567e-48aa-8423-88979d07f571.gif)

ShapelyBuffer :
Bir dynamo geometrisi (polygon,polycurve) kullanarak geometriyi oluşturan birimlerin aynı doğrutusunda kullanıcının offset mesafesine bakarak yeni bir geometri oluşturur. Kullanıcı oluşturulan geometrinin kenarlarının keskinliğine karar verebilir.
![buffer](https://user-images.githubusercontent.com/24898802/229381094-b3cdd56f-e1f2-4df1-a715-4122a93e4ec5.gif)

ShapelyDistance :
2 geometrinin birbileri arasında ki mesafeyi analiz etmemize izin verir. Bunu birbirlerine en yakın olduğu noktalardan birbirlerine dik bir çizgi çizer ve bu çizginin uzunluğu gerçek mesafeyi verir.
![distance](https://user-images.githubusercontent.com/24898802/229381212-e02d78e2-6966-4ead-87d3-70e8cf145f3b.gif)

ShapelyDifference : 
2 geometriyi birbirir arasında ki ilişikiyi analiz etmemize izin verir. Bir geometriyi diğerinden cıkarır ve kalan geometriyi içeren kapalı bir yeni geometri döndürür.
![dıfference](https://user-images.githubusercontent.com/24898802/229381253-9905c665-bee3-4bef-aa53-2c152b26fdc7.gif)

ShapelyEnvelope :
Dynamo noktalarını kullanarak noktaların oluşturacağı boundingbox'ı geometrik olarak döndürür. Yani noktalardan boundingbox geometrisi oluşturmamıza izin verir.
![envollepe](https://user-images.githubusercontent.com/24898802/229381322-071b5647-c71b-4552-9a55-2b9b478328f0.gif)

Shapelyİntersection :
2 geometriyi birbirir arasında ki ilişikiyi analiz etmemize izin verir. 2 geometriyi birbirinden cıkarır ve kapalı bir yeni geometri döndürür. Sonuç her zaman bir polygon,polycurve ve o geometriyi oluşturan noktaların listesidir.
![intersection](https://user-images.githubusercontent.com/24898802/229381389-19439f96-e741-47d1-841b-eebaf607fce9.gif)

ShapelyUnion :
2 geometriyi birbirir arasında ki ilişikiyi analiz etmemize izin verir. 2 geometriyi birleştirir ve kapalı bir yeni geometri döndürür. Sonuç her zaman bir polygon,polycurve ve o geometriyi oluşturan noktaların listesidir.
![union](https://user-images.githubusercontent.com/24898802/229381412-5057741a-c90b-415d-974c-a8c20a0ade21.gif)

ShapelyNearestPoint :
2 geometriyi birbirir arasında ki ilişikiyi analiz etmemize izin verir. 2 geometriyi birbirlerine dik olan ve en yakın noktayı bulur ve çıktı olarak bir nokta döndürür.
![neqrestpoint](https://user-images.githubusercontent.com/24898802/229381465-b78cfeb5-a583-4da8-b182-eef2f3bb281e.gif)

ShapelyOffset :
Bir dynamo geometrisi (polygon,polycurve) kullanarak geometriyi oluşturan birimlerin aynı doğrutusunda kullanıcının offset mesafesine bakarak yeni bir geometri oluşturur. Oluşturulan geometri herzaman anlamlı ve kararlıdır. Kullanıcı oluşturulan geometrinin kenarlarının keskinliğine karar verebilir.
![offset](https://user-images.githubusercontent.com/24898802/229381495-126073f0-5a25-47bc-853a-eac68c0e1faf.gif)

ShapelyOffsetParalel :
Bir dynamo geometrisi (polygon,polycurve) kullanarak geometriyi oluşturan birimlerin aynı doğrutusunda kullanıcının offset mesafesine bakarak yeni bir geometri oluşturur. Kullanıcı oluşturulan geometrinin kenarlarının keskinliğine karar verebilir. Kullanımı "ShapelyOffset" 'e göre daha hızlıdır.
![offsetparalel](https://user-images.githubusercontent.com/24898802/229381557-84494843-df91-4a0e-b755-9ebae18d71f0.gif)

ShapelyRotateSkew :
Bir 2 boyutlu dynamo geometrisi (polygon) kullanarak geometriyi tıpkı 3 boyutlu bir geometriymiş gibi X ve Y eksenlerinde yer düzleminde döndürürve yeni bir geometri oluşturur.
![rotateskew](https://user-images.githubusercontent.com/24898802/229381699-005f2256-feed-4d30-8207-eb2cd122843b.gif)

ShapelySetPrecision :
Bir dynamo geometrisi (polygon,polycurve) kullanarak geometriyi oluşturan noktaları kullanıcının belirlemesine izin veir ve yeni bir geometri döndürür. Bu geometri ana geometriden farklılaşmıştır çünkü geometriyi oluşturan köşe noktaların pozisyonu değişmiştir.
![setpreccion](https://user-images.githubusercontent.com/24898802/229381816-d245d7b3-b934-4a79-add6-a29622167783.gif)

ShapelySnap :
2 geometriyi birbirir arasında ki ilişikiyi analiz etmemize izin verir. Kullanıcının belirlediği mesafeye yaklaşan geometrinin dış hatlarına adapte olan ve kapalı bir yeni geometri döndürür. Sonuç her zaman bir polygon,polycurve ve o geometriyi oluşturan noktaların listesidir.
![snap](https://user-images.githubusercontent.com/24898802/229381896-92f2b12c-1914-42a8-b994-98207544ad79.gif)

ShapelyTouchesDistance :
Bir dynamo geometrisi (polygon,polycurve) kullanarak kullanıcı tarafından belirlenen mesafe içerisinde ki geometrileri analiz etmemize izin verir. Bunun için etrafında bulunan tüm geometrilerin dik açıda olan noktalarını bulur ve ana geometri arasında ki mesafeye analiz ederek "True" ve "False" döndürür.
![touchesdıstance](https://user-images.githubusercontent.com/24898802/229382026-7a9fd182-d27e-4aeb-8b61-ae5b25cec480.gif)














































































