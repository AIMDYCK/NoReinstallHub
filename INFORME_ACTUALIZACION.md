# Actualización 1.1.2

NoReinstallHub continúa en **Microsoft .NET 8 / Windows Forms** y mantiene su propósito: crear junctions NTFS sin mover, copiar ni borrar los archivos del usuario.

## Interfaz y uso

- Dos pasos: archivos reales y ubicación nueva del enlace.
- Temas claro, oscuro y sistema; barra nativa acorde al tema en Windows 11.
- Botones compactos y esquinas recortadas; campos de ruta con marco redondeado y foco visible.
- Selectores compactos con menús nativos, sin el marco blanco de los ComboBox.
- Español, inglés, portugués y francés; ayuda, teclado y primera apertura configurables.
- Comprobación de rutas sin efectos, progreso, resultado inline, abrir destino y copiar ruta.
- Actividad plegable y preferencias locales que no guardan rutas ni registros.

## Robustez

La creación usa API nativa Windows, sin cmd ni elevación automática. El destino se reserva de forma exclusiva. Nunca se reemplazan archivos, carpetas vacías o enlaces existentes. Las rutas superpuestas, de red o con enlaces intermedios se rechazan.

Las preferencias se guardan atómicamente y se conservan los archivos ilegibles y los campos desconocidos. No hay servicios, telemetría, escaneo de discos ni actualización automática.

## Validación

- Clean, restore y compilación Debug/Release sin errores ni advertencias.
- 1.660 comprobaciones en la revisión de 1.1.2: rutas, conservación de datos, junctions reales, colisiones, preferencias, menús y edición.
- Matriz simulada de 32 casos: cuatro idiomas, dos temas y escala 100/125/150/200 %.
- Capturas reproducibles del área cliente; las rutas mostradas son ejemplos y no crean enlaces.
- Ningún paquete NuGet directo o transitivo antes ni después de la actualización. Consultas de paquetes obsoletos y vulnerables sin referencias que actualizar.

## Versiones

App: 1.1.2. Ensamblado y paquete Store previsto: 1.1.2.0. Version.props centraliza estos valores. No se cambia .NET 8.

## Límites

La simulación no sustituye probar monitores con DPI real, Narrador y alto contraste en equipos físicos. La barra DWM no se puede validar en el entorno aislado; la captura aportada por el usuario confirma el tema oscuro en su escritorio.

La preparación MSIX no equivale a certificación: siguen pendientes identidad real de Partner Center, firma, WACK, instalación limpia y prueba bajo virtualización del paquete. Se conserva la licencia MIT existente del repositorio remoto.

.NET 8 terminó su soporte el 10 de noviembre de 2026. Se mantiene por requisito explícito y ese límite debe revisarse al distribuir. No se afirma una auditoría de seguridad del sistema operativo o del runtime.

## Historial y reversión

La publicación debe partir del historial remoto, conservar LICENSE y los archivos ajenos a esta actualización, y pasar por una rama y pull request. No se debe forzar la rama principal.

Para revertir después de integrar, usa git revert sobre los commits de la actualización, sin reset --hard ni clean indiscriminado. Cambiar de rama o revertir fuentes no elimina preferencias ni enlaces creados por el usuario.
