#!/usr/bin/env python3
"""
Script to generate PNG diagrams from PlantUML files using PlantUML server
"""

import os
import requests
import zlib
import base64
from pathlib import Path

def plantuml_encode(plantuml_text):
    """Encode PlantUML text for URL"""
    zlibbed_str = zlib.compress(plantuml_text.encode('utf-8'))
    compressed_string = zlibbed_str[2:-4]
    return base64.urlsafe_b64encode(compressed_string).decode('utf-8')

def generate_diagram(puml_file, output_dir):
    """Generate PNG diagram from PlantUML file"""
    print(f"Processing {puml_file.name}...")
    
    with open(puml_file, 'r', encoding='utf-8') as f:
        plantuml_text = f.read()
    
    # Encode the PlantUML text
    encoded = plantuml_encode(plantuml_text)
    
    # Generate PNG using PlantUML server
    url = f"http://www.plantuml.com/plantuml/png/{encoded}"
    
    try:
        response = requests.get(url, timeout=30)
        response.raise_for_status()
        
        # Save PNG file
        output_file = output_dir / f"{puml_file.stem}.png"
        with open(output_file, 'wb') as f:
            f.write(response.content)
        
        print(f"  ✓ Generated {output_file.name}")
        return True
    except Exception as e:
        print(f"  ✗ Error: {e}")
        return False

def main():
    """Main function"""
    uml_dir = Path("/home/claude/BudgetPlanner/Documentation/UML")
    output_dir = Path("/home/claude/BudgetPlanner/Documentation/Images")
    output_dir.mkdir(parents=True, exist_ok=True)
    
    # Find all .puml files
    puml_files = list(uml_dir.glob("*.puml"))
    
    if not puml_files:
        print("No .puml files found!")
        return
    
    print(f"Found {len(puml_files)} PlantUML files")
    print("-" * 50)
    
    success_count = 0
    for puml_file in sorted(puml_files):
        if generate_diagram(puml_file, output_dir):
            success_count += 1
    
    print("-" * 50)
    print(f"Successfully generated {success_count}/{len(puml_files)} diagrams")

if __name__ == "__main__":
    main()
