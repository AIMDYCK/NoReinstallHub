# Privacidad — NoReinstallHub 1.1

NoReinstallHub funciona localmente. No usa cuentas, anuncios, analítica, telemetría ni conexiones de red. No transmite archivos, rutas o preferencias a terceros.

Las rutas que escribes se usan para validar y crear la junction que solicitas. No se escanea el contenido de tus discos ni se mueven archivos. Las rutas y la actividad solo permanecen en memoria durante la sesión.

Se guardan tema, idioma y estado de la bienvenida en preferences.json dentro de LocalAppData/NoReinstallHub. Las preferencias de una aplicación MSIX pueden estar redirigidas al almacenamiento local de su paquete. Un archivo ilegible se conserva intacto. No se modifica configuración global de Windows.

La acción Copiar ruta coloca la ruta solicitada en el portapapeles de Windows. La acción Abrir destino pide al shell de Windows que abra esa carpeta. Ambas requieren pulsar sus botones; no se ejecutan al iniciar.

Los informes y capturas de tests solo se generan al ejecutar las herramientas de desarrollo, bajo artifacts/, y no se suben a ningún servicio por esos scripts.

Antes de enviar a la Store, el editor debe revisar este documento, añadir su contacto y publicar una URL de privacidad accesible según los requisitos de Partner Center. Esta entrega no publica esa URL ni inventa información del editor.
