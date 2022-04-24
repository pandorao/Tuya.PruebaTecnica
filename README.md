# Tecnologías/Framework utilizados 

.NET 6 
ASP.NET CORE API 
EntityFrameworkCore 
Swagger 
SQL Server

# Composición de la solución 

El proyecto está compuesto de 3 microservicios, los cuales deben iniciar para que la solución pueda funcionar.  

- Tuya.PruebaTecnica.DeliveryService: Microservicio encargado de todo lo relacionado con los despachos o entregas. 

- Tuya.PruebaTecnica.OrderService: Microservicio encargado de crear la orden 

- Tuya.PruebaTecnica.ProductsService: Microservicio encargado de la creacion, edicion, eliminacion y obtencion de los productos 

<img src="https://github.com/pandorao/Tuya.PruebaTecnica/blob/master/Docs/diagrama%20de%20microservicios.drawio.png"/>
 
La arquitectura de los microservicios es independiente lo que significa que cada servicio tiene su base de datos separada, pero se comunican entre ellos por el protocolo Http.

Además se crearon 2 proyectos adicionales 

- Tuya.PruebaTecnica.Models: Capa de modelos de la aplicacion 

- Tuya.PruebaTecnica.SDK: Implementacion de los diferentes microservicios en un SDK en C# 

Se utiliza en la base de datos SQL Server con EF CORE Code First. Las bases de datos y configuraciones se encuentran en el appsettings.json de cada microservicio.

#Correr la solucion

Para correr la solución debes correr los 3 proyectos en visual studio desde propiedades 

<img src="https://github.com/pandorao/Tuya.PruebaTecnica/blob/master/Docs/Captura%20de%20pantalla%20correr%20solucion.png"/>

Una vez corras los proyectos podras observar que se abrirán 3 navegadores con la documentación en swagger de cada uno de los microservicios. 

<img src="https://github.com/pandorao/Tuya.PruebaTecnica/blob/master/Docs/swagger.png" />


# Digramas de base de datos por microservicios

<img src="https://github.com/pandorao/Tuya.PruebaTecnica/blob/master/Docs/diagrama%20de%20microservicios-Page-2.drawio.png"/>

# ¿Como probar la solucion?

Para probar la solucion debes primero crear productos desde el microservicio de productos. Luego en el microservicio de ordenes podras crear una orden enviando los ids de los productos de la orden

# Consideraciones

- Por fines de prueba se corren las migraciones de EF automaticamente al momento de iniciar el proyecto.
- El servicio de ordenes se conecta tanto con el servicio de delivery como con el de producto. Para poder crear una orden envias los ids de los productos y estos deben estar creados previamente para que te pueda totalizar. Si se desconfiguran las url de los microservicios puedes configurarlos desde los appsettings.json
