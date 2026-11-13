namespace NoReinstallHub;

internal sealed class Texts(string language)
{
    internal static readonly string[] Languages = ["es", "en", "pt", "fr"];
    internal static readonly string[] LanguageNames = ["Español", "English", "Português", "Français"];
    internal static IReadOnlyDictionary<string, string[]> All => Values;
    private static readonly Dictionary<string, string[]> Values = new()
    {
        ["Tag"] = ["Carpetas conectadas, espacio organizado", "Connected folders, organized space", "Pastas conectadas, espaço organizado", "Dossiers reliés, espace organisé"],
        ["Title"] = ["Tus archivos, donde los necesitas.", "Your files, where you need them.", "Seus arquivos, onde você precisa.", "Vos fichiers, là où il faut."],
        ["Subtitle"] = ["Crea un enlace a tus archivos. Sin moverlos. Sin reinstalar.", "Create a link to your files. No moving. No reinstalling.", "Crie um link para seus arquivos. Sem mover. Sem reinstalar.", "Créez un lien vers vos fichiers. Sans déplacer ni réinstaller."],
        ["SourceTitle"] = ["Archivos reales", "Real files", "Arquivos reais", "Fichiers réels"],
        ["SourceHint"] = ["Elige la carpeta que ya contiene tus archivos.", "Choose the folder that already holds your files.", "Escolha a pasta que já contém seus arquivos.", "Choisissez le dossier qui contient déjà vos fichiers."],
        ["DestinationTitle"] = ["Nueva ubicación del enlace", "New link location", "Novo local do link", "Nouvel emplacement du lien"],
        ["DestinationHint"] = ["Escribe una ruta que todavía no exista. Los archivos seguirán en el origen.", "Enter a path that does not exist yet. Your files stay in the source.", "Digite um caminho que ainda não exista. Os arquivos ficam na origem.", "Saisissez un chemin qui n’existe pas encore. Les fichiers restent à la source."],
        ["SourcePlaceholder"] = ["Ejemplo: D:\\Juegos\\MiJuego", "Example: D:\\Games\\MyGame", "Exemplo: D:\\Jogos\\MeuJogo", "Exemple : D:\\Jeux\\MonJeu"],
        ["DestinationPlaceholder"] = ["Ejemplo: C:\\Juegos\\MiJuego", "Example: C:\\Games\\MyGame", "Exemplo: C:\\Jogos\\MeuJogo", "Exemple : C:\\Jeux\\MonJeu"],
        ["Browse"] = ["&Explorar…", "&Browse…", "&Procurar…", "&Parcourir…"],
        ["Parent"] = ["Carpeta &padre…", "&Parent folder…", "Pasta &pai…", "Dossier &parent…"],
        ["Create"] = ["&Crear enlace", "&Create link", "&Criar link", "&Créer le lien"],
        ["Validate"] = ["C&omprobar", "C&heck paths", "&Verificar", "&Vérifier"],
        ["Help"] = ["&Ayuda", "&Help", "A&juda", "&Aide"],
        ["Activity"] = ["Ver &actividad", "View &activity", "Ver &atividade", "Voir l’&activité"],
        ["HideActivity"] = ["Ocultar &actividad", "Hide &activity", "Ocultar &atividade", "Masquer l’&activité"],
        ["Clear"] = ["&Nuevo enlace", "&New link", "&Novo link", "&Nouveau lien"],
        ["Theme"] = ["Tema", "Theme", "Tema", "Thème"],
        ["Language"] = ["Idioma", "Language", "Idioma", "Langue"],
        ["System"] = ["Sistema", "System", "Sistema", "Système"],
        ["Light"] = ["Claro", "Light", "Claro", "Clair"],
        ["Dark"] = ["Oscuro", "Dark", "Escuro", "Sombre"],
        ["Ready"] = ["Todo empieza con dos carpetas.", "It starts with two folders.", "Tudo começa com duas pastas.", "Tout commence par deux dossiers."],
        ["Safe"] = ["Los destinos existentes siempre se conservan.", "Existing destinations are always preserved.", "Os destinos existentes são sempre preservados.", "Les destinations existantes sont toujours conservées."],
        ["Busy"] = ["Creando el enlace…", "Creating your link…", "Criando o link…", "Création du lien…"],
        ["Checking"] = ["Comprobando las rutas…", "Checking your paths…", "Verificando os caminhos…", "Vérification des chemins…"],
        ["Valid"] = ["Las rutas están listas. No se modificó nada.", "Your paths are ready. Nothing was changed.", "Os caminhos estão prontos. Nada foi alterado.", "Les chemins sont prêts. Rien n’a été modifié."],
        ["Success"] = ["Listo. Tu enlace ya está disponible.", "Done. Your link is ready.", "Pronto. Seu link está disponível.", "C’est fait. Votre lien est prêt."],
        ["Open"] = ["Abrir &destino", "Open &destination", "Abrir &destino", "Ouvrir la &destination"],
        ["Copy"] = ["&Copiar ruta", "&Copy path", "&Copiar caminho", "&Copier le chemin"],
        ["Copied"] = ["Ruta copiada al portapapeles.", "Path copied to clipboard.", "Caminho copiado para a área de transferência.", "Chemin copié dans le presse-papiers."],
        ["ClipboardError"] = ["No se pudo copiar. Inténtalo de nuevo.", "Could not copy. Please try again.", "Não foi possível copiar. Tente novamente.", "Impossible de copier. Réessayez."],
        ["OpenError"] = ["No se pudo abrir la carpeta. Comprueba que siga disponible.", "Could not open the folder. Check that it is still available.", "Não foi possível abrir a pasta. Verifique se está disponível.", "Impossible d’ouvrir le dossier. Vérifiez sa disponibilité."],
        ["CloseBusy"] = ["Espera a que termine la operación antes de cerrar.", "Wait for the operation to finish before closing.", "Aguarde a conclusão antes de fechar.", "Attendez la fin de l’opération avant de fermer."],
        ["Welcome"] = ["Bienvenido. Hazlo a tu manera.", "Welcome. Make it yours.", "Bem-vindo. Do seu jeito.", "Bienvenue. À votre façon."],
        ["WelcomeHint"] = ["Elige el tema y el idioma de arriba. Tus rutas no se guardan.", "Choose your theme and language above. Your paths are not saved.", "Escolha o tema e o idioma acima. Seus caminhos não são salvos.", "Choisissez le thème et la langue ci-dessus. Vos chemins ne sont pas enregistrés."],
        ["Start"] = ["&Empezar", "&Get started", "&Começar", "&Commencer"],
        ["PreferencesError"] = ["No se guardaron las preferencias. Los cambios se aplican a esta sesión.", "Preferences could not be saved. Changes apply to this session.", "As preferências não foram salvas. As mudanças valem nesta sessão.", "Les préférences n’ont pas été enregistrées. Les choix restent actifs pour cette session."],
        ["Log"] = ["Actividad de esta sesión", "Session activity", "Atividade desta sessão", "Activité de cette session"],
        ["NewLinkName"] = ["NuevoEnlace", "NewLink", "NovoLink", "NouveauLien"],
        ["HelpTitle"] = ["Cómo conectar tus carpetas", "How to connect your folders", "Como conectar suas pastas", "Comment relier vos dossiers"],
        ["HelpBody"] = ["1. Cierra las aplicaciones que usan las carpetas.\n\n2. En Archivos reales, elige la carpeta que contiene tus datos.\n\n3. En Nueva ubicación, escribe una ruta nueva. El botón Carpeta padre propone un nombre editable.\n\n4. Usa Comprobar para revisar sin cambiar nada y Crear enlace para continuar.\n\nLos archivos no se mueven ni se copian. Editar a través del enlace cambia los archivos reales. Mantén una copia de seguridad.\n\nSe admiten carpetas locales con permisos de escritura y destino NTFS. No se reemplazan archivos, carpetas o enlaces existentes, aunque estén vacíos. No uses carpetas del sistema ni rutas que atraviesen enlaces.\n\nAtajos: Alt+1 origen, Alt+2 destino, Ctrl+Enter crear, F1 ayuda. El tema y el idioma se guardan localmente; las rutas y la actividad no se guardan.", "1. Close applications using the folders.\n\n2. Under Real files, choose the folder holding your data.\n\n3. Under New link location, enter a new path. Parent folder suggests a name you can edit.\n\n4. Use Check paths to review without changes, then Create link.\n\nNo files are moved or copied. Editing through the link changes the real files. Keep a backup.\n\nUse local folders with write permission and an NTFS destination. Existing files, folders and links are never replaced, even when empty. Avoid system folders and paths through links.\n\nShortcuts: Alt+1 source, Alt+2 destination, Ctrl+Enter create, F1 help. Theme and language are saved locally; paths and activity are not saved.", "1. Feche os aplicativos que usam as pastas.\n\n2. Em Arquivos reais, escolha a pasta com seus dados.\n\n3. Em Novo local do link, digite um novo caminho. Pasta pai sugere um nome editável.\n\n4. Use Verificar para revisar sem alterações e Criar link para continuar.\n\nNenhum arquivo é movido ou copiado. Editar pelo link altera os arquivos reais. Mantenha uma cópia de segurança.\n\nUse pastas locais com permissão de escrita e destino NTFS. Arquivos, pastas e links existentes nunca são substituídos, mesmo vazios. Evite pastas do sistema e caminhos através de links.\n\nAtalhos: Alt+1 origem, Alt+2 destino, Ctrl+Enter criar, F1 ajuda. Tema e idioma são salvos localmente; caminhos e atividade não são salvos.", "1. Fermez les applications qui utilisent les dossiers.\n\n2. Dans Fichiers réels, choisissez le dossier de vos données.\n\n3. Dans Nouvel emplacement du lien, indiquez un nouveau chemin. Dossier parent propose un nom modifiable.\n\n4. Utilisez Vérifier sans rien modifier, puis Créer le lien.\n\nAucun fichier n’est déplacé ni copié. Modifier via le lien modifie les fichiers réels. Gardez une sauvegarde.\n\nUtilisez des dossiers locaux accessibles en écriture et une destination NTFS. Les fichiers, dossiers et liens existants ne sont jamais remplacés, même vides. Évitez les dossiers système et les chemins traversant des liens.\n\nRaccourcis : Alt+1 source, Alt+2 destination, Ctrl+Entrée créer, F1 aide. Le thème et la langue sont enregistrés localement, pas les chemins ni l’activité."],
        ["MissingPaths"] = ["Completa las dos rutas para continuar.", "Enter both paths to continue.", "Preencha os dois caminhos para continuar.", "Renseignez les deux chemins pour continuer."],
        ["InvalidPath"] = ["La ruta contiene un nombre o carácter no válido en Windows.", "The path contains a name or character Windows cannot use.", "O caminho contém um nome ou caractere inválido no Windows.", "Le chemin contient un nom ou caractère non valide sous Windows."],
        ["LocalPath"] = ["Usa una ruta local completa, por ejemplo D:\\Juegos\\MiJuego.", "Use a full local path, such as D:\\Games\\MyGame.", "Use um caminho local completo, como D:\\Jogos\\MeuJogo.", "Utilisez un chemin local complet, comme D:\\Jeux\\MonJeu."],
        ["RootPath"] = ["Elige una carpeta, no la raíz de la unidad.", "Choose a folder, not the drive root.", "Escolha uma pasta, não a raiz da unidade.", "Choisissez un dossier, pas la racine du lecteur."],
        ["ReservedName"] = ["La ruta contiene un nombre reservado de Windows.", "The path contains a reserved Windows name.", "O caminho contém um nome reservado do Windows.", "Le chemin contient un nom réservé de Windows."],
        ["OverlappingPaths"] = ["Las rutas deben ser distintas y ninguna puede estar dentro de la otra.", "The paths must differ and neither may be inside the other.", "Os caminhos devem ser diferentes e nenhum pode estar dentro do outro.", "Les chemins doivent être distincts et ne pas être imbriqués."],
        ["SourceMissing"] = ["El origen no existe o no está disponible. Revisa la carpeta y la unidad.", "The source is missing or unavailable. Check the folder and drive.", "A origem não existe ou não está disponível. Verifique a pasta e a unidade.", "La source est absente ou indisponible. Vérifiez le dossier et le lecteur."],
        ["DestinationExists"] = ["El destino ya existe. Elige un nombre nuevo; no se reemplazará nada.", "The destination already exists. Choose a new name; nothing will be replaced.", "O destino já existe. Escolha um nome novo; nada será substituído.", "La destination existe déjà. Choisissez un nouveau nom ; rien ne sera remplacé."],
        ["NtfsRequired"] = ["El destino debe estar en una unidad local NTFS.", "The destination must be on a local NTFS drive.", "O destino deve estar em uma unidade local NTFS.", "La destination doit être sur un lecteur local NTFS."],
        ["ExistingLink"] = ["Una ruta atraviesa un enlace existente. Elige una carpeta física.", "A path passes through an existing link. Choose a physical folder.", "Um caminho passa por um link existente. Escolha uma pasta física.", "Un chemin traverse un lien existant. Choisissez un dossier physique."],
        ["Permission"] = ["No tienes permiso en esa ubicación. Elige una carpeta donde puedas escribir.", "You do not have permission here. Choose a folder you can write to.", "Você não tem permissão nesse local. Escolha uma pasta com acesso de escrita.", "Vous n’avez pas les droits nécessaires. Choisissez un dossier accessible en écriture."],
        ["PathTooLong"] = ["La ruta es demasiado larga. Elige una ubicación más corta.", "The path is too long. Choose a shorter location.", "O caminho é muito longo. Escolha um local mais curto.", "Le chemin est trop long. Choisissez un emplacement plus court."],
        ["InUse"] = ["La carpeta está en uso. Cierra las aplicaciones que la usan y vuelve a intentar.", "The folder is in use. Close applications using it and try again.", "A pasta está em uso. Feche os aplicativos que a usam e tente novamente.", "Le dossier est utilisé. Fermez les applications concernées et réessayez."],
        ["NativeFailure"] = ["Windows no pudo crear el enlace. Revisa el destino antes de volver a intentar.", "Windows could not create the link. Check the destination before trying again.", "O Windows não conseguiu criar o link. Verifique o destino antes de tentar novamente.", "Windows n’a pas pu créer le lien. Vérifiez la destination avant de réessayer."],
        ["Unconfirmed"] = ["No se confirmó el enlace. Revisa el destino antes de repetir.", "The link was not confirmed. Check the destination before retrying.", "O link não foi confirmado. Verifique o destino antes de repetir.", "Le lien n’a pas été confirmé. Vérifiez la destination avant de réessayer."],
        ["Unexpected"] = ["No se pudo completar la operación. Revisa las rutas e inténtalo de nuevo.", "Could not complete the operation. Check the paths and try again.", "Não foi possível concluir. Verifique os caminhos e tente novamente.", "Impossible de terminer. Vérifiez les chemins et réessayez."],
    };
    private readonly int index = Math.Max(0, Array.IndexOf(Languages, language));
    internal string this[string key] => Values[key][index];
    internal string Error(Exception error) => this[error switch
    {
        JunctionException junction => junction.Code,
        UnauthorizedAccessException => "Permission",
        PathTooLongException => "PathTooLong",
        DirectoryNotFoundException => "SourceMissing",
        _ => "Unexpected"
    }];
}
