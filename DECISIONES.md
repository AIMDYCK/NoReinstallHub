# Decisiones de NoReinstallHub 1.1.2

- Mantener Microsoft .NET 8 y Windows Forms por instrucción del proyecto.
- Crear únicamente junctions; no incorporar traslado, copia, borrado ni escaneo de datos.
- Usar API nativa Windows y permisos normales. Sin shell, servicios o elevación automática.
- Conservar destinos existentes y rechazar rutas superpuestas y reparse points intermedios.
- Reservar el destino exclusivamente. Si Windows rechaza convertir la carpeta nueva, puede quedar vacía: no se limpia automáticamente para evitar borrar cambios externos.
- Mantener TextBox nativo dentro del marco redondeado. Los selectores usan botones con menús nativos, teclado y nombres accesibles.
- Recordar solo tema, idioma y bienvenida en LocalAppData. Conservar archivos dañados y propiedades desconocidas.
- Localización completa es/en/pt/fr, alto contraste y animaciones del sistema.
- No añadir dependencias de UI: no existen paquetes NuGet directos o transitivos.
- Mostrar capturas actuales del área cliente y rutas de ejemplo; no editar imágenes con IA ni publicar rutas personales.
- Conservar la licencia MIT y el historial del repositorio remoto. La copia local recibida no incluía LICENSE; esa ausencia local no significa ausencia de licencia en GitHub.
- Las anteriores restricciones de no publicar quedan sustituidas por la solicitud explícita del usuario de actualizar GitHub. Esto autoriza código, documentación, capturas y la distribución de GitHub; no autoriza publicar en Microsoft Store.
- Publicar desde una rama mediante pull request y merge normal, sin force push ni omitir políticas de protección.
- La aprobación de la Store, firma, WACK y prueba en PC limpia siguen pendientes. No inventar identidad ni certificados.
- No solicitar credenciales por chat. Usar la sesión de GitHub CLI o una integración autenticada; si el entorno no tiene acceso, informar y conservar una entrega verificable.
