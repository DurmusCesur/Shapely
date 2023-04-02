# Shapely

Shapely is a great Python library that helps us analyse and manipulate geometries. We think it is important to include this library in Dynamo. To use the Dynamoda package, you must first install the "shapely" python library with "pip install shapely". 
You can find the guide here. "https://github.com/DynamoDS/Dynamo/wiki/Customizing-Dynamo%27s-Python-3-installation" 

When you start dynamo for the first time, the package has not yet been loaded by dynamo and therefore no node belonging to the package will unfortunately not work. Don't panic and change only the "Lacing (@L2/@L1)" values of the node and force the package to run. After a few attempts the packet will work. 

Unlike dynamo, shapely uses its own geometry language. Therefore, transformations must be made between each other without losing data. In this package you will find a dynamo version of shapely geometry. Enjoy the polygons.

ShapelytoDynamo :
It converts a dynamo geometry into a "Shapely" geometry without any data loss.
![ShapelytoDynamo](https://user-images.githubusercontent.com/24898802/229380656-36acf759-17d4-4cf0-bc87-f3436fd37d56.jpg)

There is also a transformation of the shapely geometry obtained in a similar way to the dynamo geometry.

DynamotoShapely :
It converts a shapely geometry into a dynamo geometry without any data loss.
![DynamotoShapely](https://user-images.githubusercontent.com/24898802/229380730-eea47661-662a-48d4-8726-74ce6e0a87a8.jpg)

ShapelyBuffer :
Using a dynamo geometry (polygon, polycurve), it creates a new geometry by looking at the user's offset distance in the same line of the units that make up the geometry. The user can decide the sharpness of the edges of the created geometry.
![buffer](https://user-images.githubusercontent.com/24898802/229381094-b3cdd56f-e1f2-4df1-a715-4122a93e4ec5.gif)

ShapelyDifference : 
It allows us to analyse the relationship between 2 geometries. It subtracts one geometry from the other and returns a new closed geometry containing the remaining geometry.
![dıfference](https://user-images.githubusercontent.com/24898802/229381253-9905c665-bee3-4bef-aa53-2c152b26fdc7.gif)

Shapelyİntersection :
Allows us to analyse the relationship between 2 geometries. It extracts 2 geometries from each other and returns a new closed geometry. The result is always a polygon, polycurve and a list of points that make up the geometry.
![intersection](https://user-images.githubusercontent.com/24898802/229381389-19439f96-e741-47d1-841b-eebaf607fce9.gif)

ShapelyUnion :
Allows us to analyse the relationship between 2 geometries. It combines 2 geometries and returns a closed new geometry. The result is always a polygon, polycurve and a list of points that make up the geometry.
![union](https://user-images.githubusercontent.com/24898802/229381412-5057741a-c90b-415d-974c-a8c20a0ade21.gif)

ShapelySnap :
It allows us to analyse the relationship between 2 geometries. It returns a new closed geometry that adapts to the contours of the geometry approaching the distance specified by the user. The result is always a polygon, polycurve and a list of points that make up that geometry.
![snap](https://user-images.githubusercontent.com/24898802/229381896-92f2b12c-1914-42a8-b994-98207544ad79.gif)

ShapelyTouchesDistance :
Using a dynamo geometry (polygon, polycurve) allows us to analyse geometries within a distance specified by the user. For this purpose, it finds the points at right angles of all geometries around it and analyses the distance between the main geometry and returns "True" and "False".
![touchesdıstance](https://user-images.githubusercontent.com/24898802/229382026-7a9fd182-d27e-4aeb-8b61-ae5b25cec480.gif)

ShapelyArea :
It allows us to calculate the surface area of a closed geometry (polygon, polycurve).
![area](https://user-images.githubusercontent.com/24898802/229380757-fcbcb89e-18de-4698-b7e1-b65bf68c9423.jpg)

ShapelyBoundingBox :
Returns the boundingbox of a geometry as a geometry. The result is always a polgon, polycurve and a list of points that make up the geometry.
![boundıngbox](https://user-images.githubusercontent.com/24898802/229380814-0c31109f-5bed-45b9-8b74-d5e7db3cecd4.jpg)

ShapelyContains :
It allows us not to analyse the relationship of a geometry with points. It returns "True" if the points are in the geometry and "False" otherwise.
![contaıns](https://user-images.githubusercontent.com/24898802/229380863-f0166b06-a06f-48f2-b882-765616a5e5a7.gif)

ShapelyEnvelope :
It geometrically rotates the boundingbox created by the points using Dynamo points. In other words, it allows us to create boundingbox geometry from points.
![envollepe](https://user-images.githubusercontent.com/24898802/229381322-071b5647-c71b-4552-9a55-2b9b478328f0.gif)

ShapelyConvexHull :
Dynamo uses the points to create the outermost polygon that the points will form. The result is always a polygon, polycurve and a list of points that form that geometry.
![convexhull](https://user-images.githubusercontent.com/24898802/229380997-f8d15c9b-567e-48aa-8423-88979d07f571.gif)

ShapelyDistance :
It allows us to analyse the distance between 2 geometries. It draws a line perpendicular to each other at the points where they are closest to each other and the length of this line gives the actual distance.
![distance](https://user-images.githubusercontent.com/24898802/229381212-e02d78e2-6966-4ead-87d3-70e8cf145f3b.gif)

ShapelyNearestPoint :
Allows us to analyse the relationship between 2 geometries. Finds 2 geometries perpendicular to each other and the closest point and returns a point as output.
![neqrestpoint](https://user-images.githubusercontent.com/24898802/229381465-b78cfeb5-a583-4da8-b182-eef2f3bb281e.gif)

ShapelyOffset :
Using a dynamo geometry (polygon, polycurve), it creates a new geometry by looking at the user's offset distance in the same line of the units that make up the geometry. The generated geometry is always meaningful and stable. The user can decide the sharpness of the edges of the generated geometry.
![offset](https://user-images.githubusercontent.com/24898802/229381495-126073f0-5a25-47bc-853a-eac68c0e1faf.gif)

ShapelyOffsetParalel :
Using a dynamo geometry (polygon, polycurve), it creates a new geometry by looking at the user's offset distance in the same line of the units that make up the geometry. The user can decide the sharpness of the edges of the created geometry. It is faster to use than "ShapelyOffset".
![offsetparalel](https://user-images.githubusercontent.com/24898802/229381557-84494843-df91-4a0e-b755-9ebae18d71f0.gif)

ShapelyRotateSkew :
Using a 2D dynamo geometry (polygon), it rotates the geometry in the ground plane in the X and Y axes as if it were a 3D geometry and creates a new geometry.
![rotateskew](https://user-images.githubusercontent.com/24898802/229381699-005f2256-feed-4d30-8207-eb2cd122843b.gif)

ShapelySetPrecision :
Using a dynamo geometry (polygon, polycurve), it allows the user to specify the points that make up the geometry and returns a new geometry. This geometry differs from the parent geometry because the position of the vertices that make up the geometry has changed.
![setpreccion](https://user-images.githubusercontent.com/24898802/229381816-d245d7b3-b934-4a79-add6-a29622167783.gif)
















































































