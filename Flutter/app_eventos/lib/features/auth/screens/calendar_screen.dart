import 'package:app_eventos/features/auth/widgets/fights_dialog.dart';
import 'package:flutter/material.dart';
import 'package:table_calendar/table_calendar.dart';
import 'package:app_eventos/core/auth/tournament_service.dart';
import 'package:app_eventos/core/models/dto/tournament_dto.dart';

class CalendarTab extends StatefulWidget {
  const CalendarTab({super.key});

  @override
  State<CalendarTab> createState() => _CalendarTabState();
}

class _CalendarTabState extends State<CalendarTab> {
  late Future<List<TournamentDto>> _futureTournaments;
  Map<DateTime, List<TournamentDto>> _events = {};
  DateTime _focusedDay = DateTime.now();
  DateTime? _selectedDay;

  @override
  void initState() {
    super.initState();
    _futureTournaments = TournamentService().getAllTournaments();
    _futureTournaments.then((tournaments) {
      setState(() {
        _events = {};
        for (final t in tournaments) {
          final date = DateTime.parse(t.startDate);
          final day = DateTime(date.year, date.month, date.day);
          _events.putIfAbsent(day, () => []).add(t);
        }
      });
    });
  }

  List<TournamentDto> _getEventsForDay(DateTime day) {
    final key = DateTime(day.year, day.month, day.day);
    return _events[key] ?? [];
  }

  String getHoraInicio(String startDate) {
    try {
      final date = DateTime.parse(startDate);
      return '${date.hour.toString().padLeft(2, '0')}:${date.minute.toString().padLeft(2, '0')}';
    } catch (_) {
      return '';
    }
  }

  String getFecha(String dateStr) {
    try {
      final date = DateTime.parse(dateStr);
      return '${date.day.toString().padLeft(2, '0')}/${date.month.toString().padLeft(2, '0')}/${date.year}';
    } catch (_) {
      return dateStr;
    }
  }

  @override
  Widget build(BuildContext context) {
    return Stack(
      children: [
        // Fondo degradado
        Container(
          decoration: const BoxDecoration(
            gradient: LinearGradient(
              colors: [Color(0xFFB993D6), Color(0xFF8CA6DB)],
              begin: Alignment.topLeft,
              end: Alignment.bottomRight,
            ),
          ),
        ),
        FutureBuilder<List<TournamentDto>>(
          future: _futureTournaments,
          builder: (context, snapshot) {
            if (snapshot.connectionState == ConnectionState.waiting) {
              return const Center(child: CircularProgressIndicator());
            }
            return Column(
              children: [
                const SizedBox(height: 24),
                Card(
                  margin: const EdgeInsets.symmetric(horizontal: 16),
                  elevation: 6,
                  shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(18)),
                  child: Padding(
                    padding: const EdgeInsets.all(8.0),
                    child: TableCalendar<TournamentDto>(
                      firstDay: DateTime.utc(2020, 1, 1),
                      lastDay: DateTime.utc(2030, 12, 31),
                      focusedDay: _focusedDay,
                      selectedDayPredicate: (day) =>
                          _selectedDay != null &&
                          day.year == _selectedDay!.year &&
                          day.month == _selectedDay!.month &&
                          day.day == _selectedDay!.day,
                      eventLoader: _getEventsForDay,
                      calendarStyle: CalendarStyle(
                        todayDecoration: BoxDecoration(
                          color: Colors.deepPurple.shade200,
                          shape: BoxShape.circle,
                        ),
                        selectedDecoration: BoxDecoration(
                          color: Colors.deepPurple,
                          shape: BoxShape.circle,
                        ),
                        markerDecoration: BoxDecoration(
                          color: Colors.deepPurple,
                          shape: BoxShape.circle,
                        ),
                        weekendTextStyle: const TextStyle(color: Colors.deepPurple),
                        defaultTextStyle: const TextStyle(color: Colors.deepPurple),
                      ),
                      headerStyle: const HeaderStyle(
                        formatButtonVisible: false,
                        titleCentered: true,
                        titleTextStyle: TextStyle(
                          color: Colors.deepPurple,
                          fontWeight: FontWeight.bold,
                          fontSize: 18,
                        ),
                        leftChevronIcon: Icon(Icons.chevron_left, color: Colors.deepPurple),
                        rightChevronIcon: Icon(Icons.chevron_right, color: Colors.deepPurple),
                      ),
                      onDaySelected: (selectedDay, focusedDay) {
                        setState(() {
                          _selectedDay = selectedDay;
                          _focusedDay = focusedDay;
                        });
                      },
                    ),
                  ),
                ),
                const SizedBox(height: 16),
                Expanded(
                  child: Container(
                    margin: const EdgeInsets.symmetric(horizontal: 12),
                    child: _selectedDay == null
                        ? const Center(child: Text('Selecciona un día', style: TextStyle(color: Colors.white, fontSize: 18)))
                        : _getEventsForDay(_selectedDay!).isEmpty
                            ? const Center(child: Text('No hay eventos este día', style: TextStyle(color: Colors.white, fontSize: 18)))
                            : ListView(
                                children: _getEventsForDay(_selectedDay!).map((t) {
                                  String fecha = t.startDate;
                                  String fechaCorta = fecha.length >= 10 ? fecha.substring(0, 10) : fecha;
                                  return Card(
                                    color: Colors.white.withOpacity(0.95),
                                    margin: const EdgeInsets.symmetric(vertical: 10, horizontal: 4),
                                    shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(16)),
                                    elevation: 5,
                                    child: ListTile(
                                      leading: const Icon(Icons.sports_mma, color: Colors.deepPurple, size: 32),
                                      title: Text(
                                        t.sportType,
                                        style: const TextStyle(
                                          color: Colors.deepPurple,
                                          fontWeight: FontWeight.bold,
                                          fontSize: 18,
                                        ),
                                      ),
                                      subtitle: Text(
                                        'Lugar: ${t.location}\nInicio: ${getHoraInicio(t.startDate)}',
                                        style: const TextStyle(color: Colors.black87),
                                      ),
                                      onTap: () async {
                                        final fights = await TournamentService().getFightsByTournament(t.tournamentId);
                                        if (context.mounted) {
                                          showDialog(
                                            context: context,
                                            builder: (_) => FightsDialog(
                                              fights: fights,
                                              tournamentId: t.tournamentId,
                                              showParticipateButton: false, // <--- Oculta el botón
                                            ),
                                          );
                                        }
                                      },
                                    ),
                                  );
                                }).toList(),
                              ),
                  ),
                ),
              ],
            );
          },
        ),
      ],
    );
  }
}