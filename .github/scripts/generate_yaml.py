import os
import yaml

# Basisordner
base_path = "wwwroot/Enten"
output_file = "MeinBlazorProjekt/wwwroot/images.yaml"

# YAML-Datenstruktur
data = {"Enten": {}}

# Über alle Unterordner iterieren
for category in sorted(os.listdir(base_path)):
    category_path = os.path.join(base_path, category)
    if os.path.isdir(category_path):  # Nur Ordner berücksichtigen
        files = sorted(os.listdir(category_path))
        image_paths = [f"Enten/{category}/{file}" for file in files if file.endswith(('.png', '.jpg', '.jpeg'))]

        if image_paths:
            data["Enten"][category] = image_paths

# YAML-Datei schreiben
with open(output_file, "w", encoding="utf-8") as f:
    yaml.dump(data, f, default_flow_style=False, allow_unicode=True)

print(f"YAML-Datei erfolgreich erstellt: {output_file}")
