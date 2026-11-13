# NoReinstallHub 1.1.2

Conecta dos ubicaciones sin mover tus archivos: NoReinstallHub crea una **junction NTFS** en una ruta nueva que apunta a la carpeta que ya contiene tus datos.

Aplicación nativa de escritorio en **C# · Windows Forms · Microsoft .NET 8**. Sin cuentas, servicios, anuncios ni telemetría. No mueve, copia ni elimina datos.

## La nueva interfaz

Dos tarjetas numeradas distinguen **Archivos reales** y **Nueva ubicación del enlace**. Una única acción principal crea el enlace; **Comprobar** revisa las rutas sin cambiar nada. El resultado se muestra en la ventana, con botones para abrir el destino y copiar su ruta. La actividad se puede desplegar.

Tema claro, oscuro o del sistema; contraste alto del sistema; español, inglés, portugués y francés. La primera apertura conserva una configuración inicial para elegir idioma y aspecto. Solo esas preferencias se guardan: las rutas y la actividad permanecen en memoria.

Capturas de la versión 1.1.2, con rutas de ejemplo. Muestran el área de la aplicación; no se crearon enlaces para generarlas.

### Tema claro

![NoReinstallHub 1.1.2 — tema claro](docs/interfaz-clara.png)

### Tema oscuro

![NoReinstallHub 1.1.2 — tema oscuro](docs/interfaz-oscura.png)

## Descargar

Las distribuciones publicadas están en [GitHub Releases](https://github.com/AIMDYCK/NoReinstallHub/releases). Para la versión framework-dependent, extrae el ZIP completo y conserva sus archivos juntos; se requiere .NET Desktop Runtime 8.

## Requisitos e instalación local

Windows 10/11, una unidad local NTFS para el enlace y **.NET Desktop Runtime 8** para la distribución framework-dependent. Para compilar se requieren el SDK .NET y los reference packs de .NET 8. Se verificó con SDK 10.0.404, manteniendo `net8.0-windows`.

No hay paquetes NuGet externos. NoReinstallHub funciona con los permisos del usuario, sin exigir administrador ni abrir consolas. Elige ubicaciones donde tengas permiso de escritura; las carpetas protegidas pueden rechazarse.

Después de verificar, genera una carpeta portable local:

```powershell
dotnet publish NoReinstallHub.csproj -c Release --no-restore --self-contained false -o artifacts/portable
```

Abre `artifacts\portable\NoReinstallHub.exe` y conserva toda esa carpeta. Esta modalidad requiere el runtime instalado; no se anuncia como ejecutable único.

## Uso

1. En la primera apertura, selecciona tema e idioma y pulsa **Empezar**.
2. En **Archivos reales**, selecciona la carpeta que contiene tus datos.
3. En **Nueva ubicación del enlace**, escribe una ruta nueva. **Carpeta padre** propone un nombre editable.
4. Usa **Comprobar** si quieres revisar sin cambios y luego **Crear enlace**.
5. Tras el éxito puedes abrir el destino, copiar su ruta o preparar un nuevo enlace.

Ejemplo: origen `D:\Juegos\MiJuego` y enlace `C:\Juegos\MiJuego`. Ambas rutas abren los mismos datos. El programa no traslada previamente una instalación: si necesitas mover datos, prepara y verifica la copia por separado.

El destino debe ser inexistente, incluso si está vacío. No se reemplaza ni elimina nada. Se rechazan rutas iguales o anidadas, raíces, rutas de red, nombres reservados y enlaces intermedios para evitar ciclos y alias. Los caracteres válidos como `&`, `%`, `!` y `^` se admiten literalmente: ya no se usa un intérprete de comandos.

Cierra las aplicaciones que usen esas carpetas y conserva una copia de seguridad. Editar a través del enlace modifica los archivos reales. Evita carpetas del sistema e instalaciones en ejecución.

Atajos: **Alt+1** origen, **Alt+2** destino, **Ctrl+Enter** crear, **F1** ayuda. Tab recorre los controles; el foco de los botones permanece visible. La ventana usa contenido desplazable y acciones fijas para pantallas pequeñas.

## Compilación y pruebas

Para obtener el código:

```powershell
git clone https://github.com/AIMDYCK/NoReinstallHub.git
Set-Location NoReinstallHub
```

```powershell
pwsh -NoProfile -File scripts/Verificar.ps1
```

Limpia outputs conocidos de Debug/Release, restaura con la configuración local sin fuentes externas, compila ambas configuraciones tratando warnings como errores, ejecuta pruebas nativas y visuales, y consulta paquetes obsoletos y vulnerables.

El runner es un ejecutable propio sin dependencias de pruebas; **dotnet test no lo ejecuta**. Las pruebas muestran ventanas de forma breve y guardan imágenes bajo artifacts/. Cubren seguridad de rutas, conservación de datos, creación simultánea, preferencias, traducciones, contraste y una matriz simulada de escala 100/125/150/200 %. La simulación no sustituye probar monitores con DPI real ni lectores de pantalla.

Para regenerar las capturas del README, tras compilar las pruebas: `dotnet run --project tests/NoReinstallHub.Tests/NoReinstallHub.Tests.csproj -c Release --no-build --no-restore -- --capture-docs`. Usa únicamente rutas de ejemplo; conserva también docs/screenshot.png como ruta compatible para enlaces antiguos.

Para una comprobación rápida sin la matriz visual: `pwsh -NoProfile -File scripts/Verificar.ps1 -SkipVisual`.

## Errores y recuperación

Los errores se explican en el idioma elegido. Comprueba permisos, disponibilidad de las unidades y que el destino siga libre. Nunca se intenta elevar permisos automáticamente.

Si Windows rechaza convertir el directorio nuevo en junction, puede quedar una carpeta vacía recién creada por la aplicación. No se borra automáticamente: revisa el destino antes de repetir. Las operaciones nativas no se cancelan a mitad de una escritura; espera a que terminen antes de cerrar.

Las preferencias se guardan en `%LOCALAPPDATA%\NoReinstallHub\preferences.json`. Si el archivo está dañado o no se puede leer, se conserva intacto y se usan valores seguros durante esa sesión.

## Preparación Microsoft Store

Aplicación 1.1.2 y versión de paquete 1.1.2.0 centralizadas en Version.props. Se incluye una plantilla MSIX x64 y un script de empaquetado **local sin firma**. No contienen identidad Store inventada, certificados ni credenciales.

Consulta [preparación de la Store](packaging/README.md), [privacidad](PRIVACIDAD.md), [changelog](CHANGELOG.md), [plan](PLAN_ACTUALIZACION.md) e [informe](INFORME_ACTUALIZACION.md). No se afirma aprobación ni certificación de Microsoft Store; la publicación en GitHub es independiente.

Se conserva .NET 8 por requisito del proyecto; su soporte terminó el 10 de noviembre de 2026. Ese límite debe revisarse al distribuir, sin migrar automáticamente a otra plataforma. [Política de Microsoft](https://dotnet.microsoft.com/en-us/platform/support/policy).

Repositorio de referencia: [AIMDYCK/NoReinstallHub](https://github.com/AIMDYCK/NoReinstallHub). Licencia **MIT**. Consulta [LICENSE](LICENSE), conservado del repositorio.
