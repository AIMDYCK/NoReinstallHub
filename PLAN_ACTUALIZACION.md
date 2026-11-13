# Plan aplicado — NoReinstallHub 1.1.2

La actualización conserva Microsoft .NET 8 y Windows Forms.

1. Adoptar jerarquía, espaciado, capas y temas inspirados en Fluent, sin introducir otro framework.
2. Mantener dos pasos: archivos reales y ubicación nueva del enlace. Compactar botones y selectores; redondear los campos de ruta.
3. Localizar interfaz y ayuda en es/en/pt/fr. Añadir teclado, alto contraste y configuración inicial.
4. Sustituir mklink por API nativa de Windows, sin elevar permisos ni ejecutar consolas. Conservar destinos existentes.
5. Guardar solo tema, idioma y bienvenida; proteger archivos de preferencias ilegibles y campos desconocidos.
6. Verificar Debug/Release, junctions reales, colisiones y 32 combinaciones de idioma, tema y escala simulada.
7. Actualizar documentación y capturas reproducibles del área cliente, con rutas de ejemplo.
8. Conservar LICENSE y el historial remoto. Publicar desde rama/PR sin force push; mantener binarios fuera del árbol fuente.
9. Preparar versión 1.1.2 y plantilla Store 1.1.2.0 sin afirmar certificación, firma o instalación limpia.

Referencias de diseño y plataforma:
- [Principios Windows](https://learn.microsoft.com/en-us/windows/apps/design/design-principles)
- [Tipografía](https://learn.microsoft.com/en-us/windows/apps/design/signature-experiences/typography)
- [WinUI Gallery, referencia visual](https://github.com/microsoft/WinUI-Gallery)
- [Configuración WinForms .NET](https://learn.microsoft.com/en-us/dotnet/desktop/winforms/application-configuration)
- [Preparar MSIX](https://learn.microsoft.com/en-us/windows/msix/desktop/desktop-to-uwp-prepare)
- [Políticas Store](https://learn.microsoft.com/en-us/windows/apps/publish/store-policies)
- [Reparse data](https://learn.microsoft.com/en-us/windows-hardware/drivers/ddi/ntifs/ns-ntifs-_reparse_data_buffer)
- [Atributos DWM](https://learn.microsoft.com/en-us/windows/win32/api/dwmapi/ne-dwmapi-dwmwindowattribute)
