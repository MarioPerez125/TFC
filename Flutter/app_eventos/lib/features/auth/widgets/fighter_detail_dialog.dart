import 'package:app_eventos/core/models/dto/fighter_for_friend_dto.dart';
import 'package:flutter/material.dart';

class FighterDetailDialog extends StatelessWidget {
  final FighterForFriendDto fighter;

  const FighterDetailDialog({super.key, required this.fighter});

  @override
  Widget build(BuildContext context) {
    return AlertDialog(
      shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(18)),
      title: Row(
        children: [
          const Icon(Icons.sports_mma, color: Colors.deepPurple, size: 32),
          const SizedBox(width: 12),
          Expanded(
            child: Text(
              '${fighter.name} ${fighter.lastName}',
              style: const TextStyle(fontWeight: FontWeight.bold),
            ),
          ),
        ],
      ),
      content: SingleChildScrollView(
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            _ProfileRow(icon: Icons.account_circle, label: 'Usuario', value: fighter.username),
            _ProfileRow(icon: Icons.cake, label: 'Nacimiento', value: fighter.birthDate),
            _ProfileRow(icon: Icons.location_city, label: 'Ciudad', value: fighter.city),
            _ProfileRow(icon: Icons.flag, label: 'País', value: fighter.country),
            const Divider(),
            _ProfileRow(icon: Icons.sports_mma, label: 'Categoría de peso', value: fighter.weightClass),
            _ProfileRow(icon: Icons.height, label: 'Altura', value: fighter.height != null ? '${fighter.height} cm' : ''),
            _ProfileRow(icon: Icons.open_with, label: 'Alcance', value: fighter.reach != null ? '${fighter.reach} cm' : ''),
            _ProfileRow(icon: Icons.emoji_events, label: 'Victorias', value: '${fighter.wins}'),
            _ProfileRow(icon: Icons.close, label: 'Derrotas', value: '${fighter.losses}'),
            _ProfileRow(icon: Icons.remove, label: 'Empates', value: '${fighter.draws}'),
          ],
        ),
      ),
      actions: [
        TextButton(
          onPressed: () => Navigator.pop(context),
          child: const Text('Cerrar'),
        ),
      ],
    );
  }
}

class _ProfileRow extends StatelessWidget {
  final IconData icon;
  final String label;
  final String value;

  const _ProfileRow({
    required this.icon,
    required this.label,
    required this.value,
  });

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 4),
      child: Row(
        children: [
          Icon(icon, color: Colors.deepPurple, size: 22),
          const SizedBox(width: 10),
          Expanded(
            child: Text(
              label,
              style: const TextStyle(fontWeight: FontWeight.w600),
            ),
          ),
          Expanded(
            flex: 2,
            child: Text(
              value,
              textAlign: TextAlign.right,
            ),
          ),
        ],
      ),
    );
  }
}

class AppTextField extends StatelessWidget {
  final TextEditingController controller;
  final String label;
  final IconData icon;
  final String? Function(String?)? validator;

  const AppTextField({
    super.key,
    required this.controller,
    required this.label,
    required this.icon,
    this.validator,
  });

  @override
  Widget build(BuildContext context) {
    return TextFormField(
      controller: controller,
      validator: validator,
      decoration: InputDecoration(
        labelText: label,
        prefixIcon: Icon(icon, color: Colors.deepPurple),
        border: OutlineInputBorder(
          borderRadius: BorderRadius.circular(18),
          borderSide: const BorderSide(color: Colors.deepPurple),
        ),
        focusedBorder: OutlineInputBorder(
          borderRadius: BorderRadius.circular(18),
          borderSide: const BorderSide(color: Colors.deepPurple, width: 2),
        ),
      ),
    );
  }
}

class AppButton extends StatelessWidget {
  final String label;
  final IconData icon;
  final bool loading;
  final VoidCallback onPressed;

  const AppButton({
    super.key,
    required this.label,
    required this.icon,
    required this.loading,
    required this.onPressed,
  });

  @override
  Widget build(BuildContext context) {
    return ElevatedButton.icon(
      onPressed: loading ? null : onPressed,
      icon: loading
          ? const CircularProgressIndicator(
              valueColor: AlwaysStoppedAnimation<Color>(Colors.white),
              strokeWidth: 2.0,
            )
          : Icon(icon),
      label: Text(label),
      style: ElevatedButton.styleFrom(
        foregroundColor: Colors.white, shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(18)), backgroundColor: Colors.deepPurple,
        padding: const EdgeInsets.symmetric(vertical: 12, horizontal: 20),
      ),
    );
  }
}