# Preparación Microsoft Store — 1.1.2.0

La copia inicial no tenía MSIX ni identidad Store. Esta entrega añade una plantilla y preparación local; **no certifica** aceptación, instalación ni firma.

La aplicación usa asInvoker, crea junctions con API de Windows y no lanza cmd, PowerShell, servicios o elevación. La capacidad runFullTrust es restringida y debe justificarse ante Partner Center. No se declara allowElevation ni broadFileSystemAccess.

## Pasos pendientes para una entrega real
1. Reservar o recuperar la identidad real de NoReinstallHub en Partner Center. Usar Name y Publisher exactos, sin inventarlos.
2. Generar un publish **win-x64 self-contained** con .NET 8. Requiere los runtime packs 8 correspondientes. Si no están instalados, cualquier restauración debe descargar paquetes únicamente a una carpeta del proyecto desde una fuente oficial; no instalar software del sistema desde los scripts de revisión.
3. Proporcionar Windows SDK / makeappx ya instalado y la carpeta de publish. El script verifica que contiene hostfxr.dll y System.Private.CoreLib.dll.
4. Ejecutar localmente:
```powershell
pwsh -NoProfile -File packaging/Preparar-Store.ps1 -IdentityName "IDENTIDAD_REAL" -Publisher "CN=VALOR_REAL" -PublisherDisplayName "EDITOR_REAL" -PublishDirectory "RUTA_AL_PUBLISH_SELF_CONTAINED" -MakeAppxPath "RUTA_WINDOWS_SDK\makeappx.exe"
```
5. El resultado será un MSIX **sin firma**, bajo artifacts/store-GUID. No lo instala ni sube. Para uso real faltan firma, instalación limpia, WACK y certificación.
6. Revisar capturas, metadatos es/en/pt/fr, requisitos del runtime, privacidad y capacidad runFullTrust antes de enviar manualmente.

Version.props centraliza Version 1.1.2, FileVersion 1.1.2.0 y StorePackageVersion 1.1.2.0. El manifiesto generado usa esa versión; el cuarto componente queda en cero.

Los assets se derivan del icono existente del proyecto, sin descargar ni introducir marcas ajenas. La plantilla no declara soporte de arquitecturas no probadas.

## Límites de esta revisión
No se dispuso de identidad, firma o herramientas makeappx/WACK verificables. No se generó un MSIX certificado ni se descargó un runtime. Las pruebas normales de escritorio no demuestran automáticamente el comportamiento bajo virtualización MSIX: hay que probar enlaces entre volúmenes y directorios fuera del paquete con una cuenta estándar.

Referencias: [preparar MSIX](https://learn.microsoft.com/en-us/windows/msix/desktop/desktop-to-uwp-prepare), [capacidades](https://learn.microsoft.com/en-us/windows/apps/package-and-deploy/app-capability-declarations), [políticas Store](https://learn.microsoft.com/en-us/windows/apps/publish/store-policies).
