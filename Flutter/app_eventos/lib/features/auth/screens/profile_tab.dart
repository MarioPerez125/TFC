import 'package:app_eventos/core/auth/auth_provider.dart';
import 'package:flutter/material.dart';
import 'package:http/http.dart' as _apiClient;
import 'package:provider/provider.dart';
import 'package:app_eventos/core/auth/auth_service.dart';
import 'package:app_eventos/core/models/dto/register_dto.dart';
import 'package:app_eventos/core/models/dto/fighter_dto.dart';
import 'package:app_eventos/core/models/dto/auth_dto.dart';

class ProfileTab extends StatelessWidget {
  const ProfileTab({super.key});

  Future<void> _showResultDialog(BuildContext context, bool success, String message) async {
    await showDialog(
      context: context,
      builder: (_) => AlertDialog(
        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(18)),
        title: Row(
          children: [
            Icon(
              success ? Icons.check_circle : Icons.error,
              color: success ? Colors.green : Colors.red,
              size: 32,
            ),
            const SizedBox(width: 12),
            Text(success ? '¡Éxito!' : 'Error'),
          ],
        ),
        content: Text(message, style: const TextStyle(fontSize: 16)),
        actions: [
          TextButton(
            onPressed: () => Navigator.pop(context),
            child: const Text('Cerrar'),
          ),
        ],
      ),
    );
  }

  Future<void> _solicitarOrganizador(
    BuildContext context,
    AuthProvider authProvider,
  ) async {
    final user = authProvider.user;
    if (user == null) return;

    // Pide la contraseña al usuario para mayor seguridad
    final passwordController = TextEditingController();
    final result = await showDialog<String>(
      context: context,
      builder: (dialogContext) {
        return AlertDialog(
          shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(18)),
          title: const Text('Solicitar ser organizador'),
          content: TextField(
            controller: passwordController,
            decoration: const InputDecoration(labelText: 'Confirma tu contraseña'),
            obscureText: true,
          ),
          actions: [
            TextButton(
              onPressed: () => Navigator.of(dialogContext).pop(null),
              child: const Text('Cancelar'),
            ),
            ElevatedButton(
              style: ElevatedButton.styleFrom(
                backgroundColor: Colors.indigo,
                shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
              ),
              onPressed: () {
                Navigator.of(dialogContext).pop(passwordController.text);
              },
              child: const Text('Solicitar'),
            ),
          ],
        );
      },
    );

    if (result != null && result.isNotEmpty) {
      final authDto = AuthDto(
        username: user.username,
        password: result,
      );
      final success = await AuthService().registerAsOrganizer(authDto);
      if (context.mounted) {
        await _showResultDialog(
          context,
          success,
          success
              ? '¡Solicitud para ser organizador enviada con éxito!'
              : 'No se pudo enviar la solicitud.',
        );
      }
    }
  }

  Future<void> _solicitarPeleador(
    BuildContext context,
    AuthProvider authProvider,
  ) async {
    final user = authProvider.user;
    if (user == null) return;

    final weightController = TextEditingController();
    final heightController = TextEditingController();
    final reachController = TextEditingController();

    final result = await showDialog<bool>(
      context: context,
      builder: (dialogContext) {
        return AlertDialog(
          shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(18)),
          title: const Text('Solicitar ser peleador'),
          content: Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              TextField(
                controller: weightController,
                decoration: const InputDecoration(
                  labelText: 'Categoría de peso',
                ),
              ),
              TextField(
                controller: heightController,
                decoration: const InputDecoration(labelText: 'Altura (cm)'),
                keyboardType: TextInputType.number,
              ),
              TextField(
                controller: reachController,
                decoration: const InputDecoration(labelText: 'Alcance (cm)'),
                keyboardType: TextInputType.number,
              ),
            ],
          ),
          actions: [
            TextButton(
              onPressed: () => Navigator.of(dialogContext).pop(false),
              child: const Text('Cancelar'),
            ),
            ElevatedButton(
              style: ElevatedButton.styleFrom(
                backgroundColor: Colors.deepPurple,
                shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12)),
              ),
              onPressed: () async {
                final fighterDTO = FighterDto(
                  userId: user.userId ?? 0,
                  weightClass: weightController.text,
                  height: int.tryParse(heightController.text) ?? 0,
                  reach: int.tryParse(reachController.text) ?? 0,
                  wins: 0,
                  losses: 0,
                  draws: 0,
                );
                final success = await AuthService().registerAsFighter(fighterDTO);
                if (success) {
                  await authProvider.refreshUser(); // <--- Esto actualiza el rol en la UI
                }
                Navigator.of(dialogContext).pop(success);
              },
              child: const Text('Solicitar'),
            ),
          ],
        );
      },
    );

    if (result != null && context.mounted) {
      if (result) {
        await authProvider.refreshUser();
      }
      await _showResultDialog(
        context,
        result,
        result
            ? '¡Solicitud para ser peleador enviada con éxito!'
            : 'No se pudo enviar la solicitud.',
      );
    }
  }

  @override
  Widget build(BuildContext context) {
    final authProvider = Provider.of<AuthProvider>(context, listen: false);
    final user = authProvider.user;
    if (user == null) {
      return const Center(child: Text('No hay usuario logueado'));
    }
    return ListView(
      padding: const EdgeInsets.all(16),
      children: [
        Center(
          child: CircleAvatar(
            radius: 48,
            backgroundColor: Colors.deepPurple.shade100,
            child: const Icon(Icons.person, size: 60, color: Colors.deepPurple),
          ),
        ),
        const SizedBox(height: 24),
        Card(
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(18),
          ),
          elevation: 4,
          child: Padding(
            padding: const EdgeInsets.symmetric(vertical: 24, horizontal: 20),
            child: Column(
              children: [
                _ProfileRow(
                  icon: Icons.person,
                  label: 'Nombre',
                  value: user.name ?? '',
                ),
                const Divider(),
                _ProfileRow(
                  icon: Icons.person_outline,
                  label: 'Apellido',
                  value: user.lastName ?? '',
                ),
                const Divider(),
                _ProfileRow(
                  icon: Icons.account_circle,
                  label: 'Usuario',
                  value: user.username ?? '',
                ),
                const Divider(),
                _ProfileRow(
                  icon: Icons.email,
                  label: 'Email',
                  value: user.email ?? '',
                ),
                const Divider(),
                _ProfileRow(
                  icon: Icons.phone,
                  label: 'Teléfono',
                  value: user.phone ?? '',
                ),
                const Divider(),
                _ProfileRow(
                  icon: Icons.cake,
                  label: 'Nacimiento',
                  value: user.birthDate != null
                      ? '${user.birthDate!.day.toString().padLeft(2, '0')}/'
                        '${user.birthDate!.month.toString().padLeft(2, '0')}/'
                        '${user.birthDate!.year}'
                      : '',
                ),
                const Divider(),
                _ProfileRow(
                  icon: Icons.location_city,
                  label: 'Ciudad',
                  value: user.city ?? '',
                ),
                const Divider(),
                _ProfileRow(
                  icon: Icons.flag,
                  label: 'País',
                  value: user.country ?? '',
                ),
              ],
            ),
          ),
        ),
        const SizedBox(height: 32),
        Row(
          children: [
            Expanded(
              child: ElevatedButton.icon(
                icon: const Icon(Icons.sports_mma, size: 28),
                label: const Padding(
                  padding: EdgeInsets.symmetric(vertical: 16),
                  child: Text(
                    'Solicitar ser Peleador',
                    style: TextStyle(fontSize: 16, fontWeight: FontWeight.bold),
                  ),
                ),
                style: ElevatedButton.styleFrom(
                  backgroundColor: Colors.deepPurple,
                  foregroundColor: Colors.white,
                  elevation: 8,
                  shadowColor: Colors.deepPurple.withOpacity(0.3),
                  shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(18),
                  ),
                ),
                onPressed: () => _solicitarPeleador(context, authProvider),
              ),
            ),
            const SizedBox(width: 16),
            Expanded(
              child: ElevatedButton.icon(
                icon: const Icon(Icons.emoji_events, size: 28),
                label: const Padding(
                  padding: EdgeInsets.symmetric(vertical: 16),
                  child: Text(
                    'Solicitar ser Organizador',
                    style: TextStyle(fontSize: 16, fontWeight: FontWeight.bold),
                  ),
                ),
                style: ElevatedButton.styleFrom(
                  backgroundColor: Colors.indigo,
                  foregroundColor: Colors.white,
                  elevation: 8,
                  shadowColor: Colors.indigo.withOpacity(0.3),
                  shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(18),
                  ),
                ),
                onPressed: () => _solicitarOrganizador(context, authProvider),
              ),
            ),
          ],
        ),
        const SizedBox(height: 24),
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
    return Row(
      children: [
        Icon(icon, color: Colors.deepPurple, size: 26),
        const SizedBox(width: 16),
        Expanded(
          child: Text(
            label,
            style: const TextStyle(
              fontWeight: FontWeight.w600,
              fontSize: 16,
              color: Colors.deepPurple,
            ),
          ),
        ),
        Expanded(
          flex: 2,
          child: Text(
            value,
            textAlign: TextAlign.right,
            style: const TextStyle(fontSize: 16),
          ),
        ),
      ],
    );
  }
}
