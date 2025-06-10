import 'package:app_eventos/core/auth/auth_provider.dart';
import 'package:app_eventos/features/auth/widgets/fighter_request_dialog.dart';
import 'package:flutter/material.dart';
import 'package:http/http.dart' as _apiClient;
import 'package:provider/provider.dart';
import 'package:app_eventos/core/auth/service.dart';
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
      final userDto = await Service().registerAsOrganizer(authDto);
      final bool success = userDto != null;
      if (success) {
        await authProvider.setUserFromDto(userDto!);
      }
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

    final result = await showDialog<Map<String, dynamic>>(
      context: context,
      builder: (context) => const FighterRequestDialog(),
    );

    if (result != null) {
      final fighterDTO = FighterDto(
        userId: user.userId ?? 0,
        weightClass: result['category'],
        height: result['height'],
        reach: result['reach'],
        wins: 0,
        losses: 0,
        draws: 0,
      );
      final userDto = await Service().registerAsFighter(fighterDTO);
      final bool success = userDto != null;
      if (success) {
        await authProvider.setUserFromDto(userDto!);
      }
      if (context.mounted) {
        await _showResultDialog(
          context,
          success,
          success
              ? '¡Solicitud para ser peleador enviada con éxito!'
              : 'No se pudo enviar la solicitud.',
        );
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    final authProvider = Provider.of<AuthProvider>(context);
    final user = authProvider.user;
    if (user == null) {
      return const Center(child: Text('No hay usuario logueado'));
    }

    final isUser = user.role == null || user.role == 'User';
    final isFighter = user.role == 'Fighter';
    final isOrganizer = user.role == 'Organizer';

    List<Widget> actionButtons = [];

    if (isUser || isFighter) {
      actionButtons.add(
        _ActionButton(
          icon: Icons.emoji_events,
          label: 'Solicitar ser Organizador',
          color: Colors.indigo,
          onPressed: () => _solicitarOrganizador(context, authProvider),
        ),
      );
    }
    if (isUser || isOrganizer) {
      actionButtons.add(
        _ActionButton(
          icon: Icons.sports_mma,
          label: 'Solicitar ser Peleador',
          color: Colors.deepPurple,
          onPressed: () => _solicitarPeleador(context, authProvider),
        ),
      );
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
        if (isFighter) ...[
          const SizedBox(height: 24),
          FutureBuilder<FighterDto?>(
            future: Service().getFighterInfo(user.userId!),
            builder: (context, snapshot) {
              if (snapshot.connectionState == ConnectionState.waiting) {
                return const Center(child: CircularProgressIndicator());
              }
              if (!snapshot.hasData || snapshot.data == null) {
                return const Center(child: Text('No se encontró información de peleador.'));
              }
              final fighter = snapshot.data!;
              return Card(
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(18),
                ),
                elevation: 4,
                child: Padding(
                  padding: const EdgeInsets.symmetric(vertical: 20, horizontal: 20),
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      const Text(
                        'Información de Peleador',
                        style: TextStyle(
                          fontWeight: FontWeight.bold,
                          fontSize: 18,
                          color: Colors.deepPurple,
                        ),
                      ),
                      const Divider(),
                      _ProfileRow(
                        icon: Icons.sports_mma,
                        label: 'Categoría de peso',
                        value: fighter.weightClass,
                      ),
                      _ProfileRow(
                        icon: Icons.height,
                        label: 'Altura',
                        value: '${fighter.height} cm',
                      ),
                      _ProfileRow(
                        icon: Icons.open_with,
                        label: 'Alcance',
                        value: '${fighter.reach} cm',
                      ),
                      _ProfileRow(
                        icon: Icons.emoji_events,
                        label: 'Victorias',
                        value: '${fighter.wins}',
                      ),
                      _ProfileRow(
                        icon: Icons.close,
                        label: 'Derrotas',
                        value: '${fighter.losses}',
                      ),
                      _ProfileRow(
                        icon: Icons.remove,
                        label: 'Empates',
                        value: '${fighter.draws}',
                      ),
                    ],
                  ),
                ),
              );
            },
          ),
        ],
        const SizedBox(height: 32),
        if (actionButtons.isNotEmpty)
          Row(
            mainAxisAlignment: actionButtons.length == 2
                ? MainAxisAlignment.spaceBetween
                : MainAxisAlignment.center,
            children: actionButtons
                .map((btn) => Expanded(child: btn))
                .toList(),
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

class _ActionButton extends StatelessWidget {
  final IconData icon;
  final String label;
  final Color color;
  final VoidCallback onPressed;

  const _ActionButton({
    required this.icon,
    required this.label,
    required this.color,
    required this.onPressed,
  });

  @override
  Widget build(BuildContext context) {
    return ElevatedButton.icon(
      icon: Icon(icon, size: 28, color: Colors.white),
      label: Padding(
        padding: const EdgeInsets.symmetric(vertical: 16),
        child: Text(
          label,
          style: const TextStyle(fontSize: 16, fontWeight: FontWeight.bold, color: Colors.white),
        ),
      ),
      style: ElevatedButton.styleFrom(
        backgroundColor: color,
        elevation: 8,
        shadowColor: color.withOpacity(0.3),
        shape: RoundedRectangleBorder(
          borderRadius: BorderRadius.circular(18),
        ),
      ),
      onPressed: onPressed,
    );
  }
}
