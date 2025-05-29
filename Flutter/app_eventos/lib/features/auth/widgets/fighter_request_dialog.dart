import 'package:flutter/material.dart';

class FighterRequestDialog extends StatefulWidget {
  const FighterRequestDialog({super.key});

  @override
  State<FighterRequestDialog> createState() => _FighterRequestDialogState();
}

class _FighterRequestDialogState extends State<FighterRequestDialog> {
  final List<String> _categories = [
    'Flyweight',
    'Bantamweight',
    'Featherweight',
    'Lightweight',
    'Welterweight',
    'Middleweight',
    'Light Heavyweight',
    'Heavyweight',
  ];

  String? _selectedCategory;
  final TextEditingController _heightController = TextEditingController();
  final TextEditingController _reachController = TextEditingController();

  @override
  void dispose() {
    _heightController.dispose();
    _reachController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return AlertDialog(
      shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(18)),
      title: const Text('Solicitar ser peleador'),
      content: Column(
        mainAxisSize: MainAxisSize.min,
        children: [
          DropdownButtonFormField<String>(
            decoration: const InputDecoration(
              labelText: 'Categoría de peso',
              border: OutlineInputBorder(),
            ),
            value: _selectedCategory,
            items: _categories
                .map((cat) => DropdownMenuItem(
                      value: cat,
                      child: Text(cat),
                    ))
                .toList(),
            onChanged: (value) => setState(() => _selectedCategory = value),
            validator: (value) =>
                value == null ? 'Selecciona una categoría' : null,
          ),
          const SizedBox(height: 16),
          TextField(
            controller: _heightController,
            decoration: const InputDecoration(
              labelText: 'Altura (cm)',
              border: OutlineInputBorder(),
            ),
            keyboardType: TextInputType.number,
          ),
          const SizedBox(height: 16),
          TextField(
            controller: _reachController,
            decoration: const InputDecoration(
              labelText: 'Alcance (cm)',
              border: OutlineInputBorder(),
            ),
            keyboardType: TextInputType.number,
          ),
        ],
      ),
      actions: [
        TextButton(
          onPressed: () => Navigator.of(context).pop(null),
          child: const Text('Cancelar'),
        ),
        ElevatedButton(
          onPressed: () {
            if (_selectedCategory == null ||
                _heightController.text.isEmpty ||
                _reachController.text.isEmpty) {
              ScaffoldMessenger.of(context).showSnackBar(
                const SnackBar(content: Text('Completa todos los campos')),
              );
              return;
            }
            Navigator.of(context).pop({
              'category': _selectedCategory,
              'height': int.tryParse(_heightController.text) ?? 0,
              'reach': int.tryParse(_reachController.text) ?? 0,
            });
          },
          child: const Text('Solicitar'),
        ),
      ],
    );
  }
}