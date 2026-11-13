# NoReinstallHub

C# / Microsoft .NET 8 / Windows Forms. No cambiar framework, propósito ni nombre del producto. La instrucción de mantener .NET 8 prevalece sobre recomendaciones históricas de migración.

## Verificación
- Completa: `pwsh -NoProfile -File scripts/Verificar.ps1`.
- Rápida sin capturas: `pwsh -NoProfile -File scripts/Verificar.ps1 -SkipVisual`.
- El script limpia outputs conocidos, restaura con NuGet.Config, compila Debug y Release con -warnaserror y ejecuta el runner, más consultas NuGet.
- No usar dotnet test como sustituto del runner ejecutable.
- Publicación local: `dotnet publish NoReinstallHub.csproj -c Release --no-restore --self-contained false -o artifacts/portable`.
- Mantener fuentes fuera de bin/obj/artifacts. Una actualización preparada bajo obj es una entrega temporal, no una nueva arquitectura.

## Arquitectura
- JunctionService: normalización, validación y operación asíncrona.
- NativeJunction: creación exclusiva y reparse data NTFS mediante SafeFileHandle y API Win32. No shells, servicios ni elevación.
- MainForm/Designer: layout adaptable, dos pasos, resultado inline y acciones. UI fuera de operaciones de disco.
- UiTheme: paletas, botones, iconos y selectores; conservar accesibilidad de los botones, menús nativos y TextBox interior. Respetar alto contraste y animación del sistema.
- Texts: todos los textos de usuario en es/en/pt/fr. Error codes se traducen en UI, no mensajes técnicos mezclados.
- AppPreferences: solo tema/idioma/bienvenida; JSON atómico en LocalAppData. No guardar rutas, no sobrescribir un archivo ilegible, conservar campos desconocidos.
- Version.props: versión de ensamblado y paquete Store. Sincronizar changelog y documentación.
- packaging: plantilla MSIX, identidad suministrada desde Partner Center, nunca simular certificación.

## Convenciones
Nullable, indentación según .editorconfig. Recursos GDI, SafeFileHandle, RegistryKey y timers liberados al terminar.
No reemplazar ni borrar destinos, ni siquiera vacíos. No mover datos del usuario. No atravesar reparse points. No crear rutas en segundo plano sin una acción explícita del usuario.
Las pruebas usan un directorio GUID bajo artifacts y retiran únicamente sus enlaces antes de limpiar sus propios datos.
La creación usa CreateDirectory exclusivo: no cambiarlo por Directory.CreateDirectory para reservar el destino.
No hay dependencias NuGet; no añadir paquetes de UI por moda. Al introducir una dependencia, habilitar conscientemente la fuente requerida, revisar licencia y vulnerabilidades.
No subir, publicar, firmar, instalar paquetes del sistema ni cambiar seguridad de Windows sin autorización. La solicitud explícita de actualizar GitHub autoriza esa publicación, sin extenderse a Microsoft Store.
Las capturas simuladas no certifican DPI real, lector de pantalla, Store ni instalación limpia.

## Git
Trabajar en una rama de actualización y conservar la historia remota y LICENSE. No usar push --force ni omitir las políticas de protección. Revisar el diff, verificar y publicar mediante pull request con el commit exacto probado. No incluir binarios, cachés, preferencias o secretos en el repositorio; los ZIP de distribución pertenecen a Releases.
