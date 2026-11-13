# Changelog

## 1.1.2 — 2026-11-12
- Recorte físico redondeado de los botones para eliminar residuos en las esquinas.
- Campos de ruta sin borde nativo, dentro de un marco redondeado con foco visible.
- Selectores de tema e idioma compactos, con flecha dibujada y menú nativo, sin borde blanco de ComboBox.
- Se conservan teclado, selección, traducciones y persistencia de preferencias.

## 1.1.1 — 2026-11-12
- Botones compactos, tarjetas y ventana con menor espaciado.
- Barra de título nativa de Windows 11 sincronizada con claro/oscuro/sistema y alto contraste.
- Fondo de botones corregido para evitar esquinas y bordes residuales.
- Pruebas de altura de botones y lectura del atributo de color de la barra nativa.

## 1.1.0 — 2026-11-12
### Interfaz
- Diseño de dos pasos inspirado en Fluent: jerarquía tipográfica, tarjetas, iconos de carpeta, espaciado y acción principal.
- Temas claro, oscuro y sistema; respeto del alto contraste y de las animaciones del sistema.
- Español, inglés, portugués y francés completos.
- Configuración de primera apertura, preferencias locales, ayuda localizada, atajos y contenido desplazable.

### Funcionalidad
- Comprobar rutas sin crear nada, progreso, resultado inline, abrir destino y copiar ruta.
- Actividad de sesión plegable y sin persistencia.
- Junctions mediante API nativa de Windows; sin cmd.exe, PowerShell o elevación automática.
- Creación exclusiva contra colisiones y soporte literal de &, %, ! y ^.
- Configuración inválida y campos desconocidos conservados.

### Calidad y distribución
- Pruebas NTFS reales y matriz visual simulada de cuatro idiomas, dos temas y cuatro escalas.
- Clean/restore/build en Debug y Release con warnings como errores.
- Paquetes NuGet directos/transitivos: ninguno antes y después.
- Versión de paquete prevista: 1.1.0.0; plantilla MSIX y preparación local sin firma.
- Se mantiene Microsoft .NET 8 por requisito explícito. Sin publicación ni push.

## 1.0.0
- Base original: creación de junctions con mklink /J y Windows Forms.
- La revisión anterior de robustez quedó documentada en INFORME_MEJORAS.md; sus cambios aún no tenían commits propios al iniciar 1.1.
