﻿using Routini.MAUI.Entities.Routines;
using Routini.MAUI.Shared.Databases;
using SQLite;

namespace Routini.MAUI.Features.ListRoutines
{
    public class GetAllRoutinesQuery
    {
        private readonly ISqliteConnectionFactory _sqliteConnectionFactory;

        public GetAllRoutinesQuery(ISqliteConnectionFactory sqliteConnectionFactory)
        {
            _sqliteConnectionFactory = sqliteConnectionFactory;
        }

        public async Task<IEnumerable<Routine>> Execute()
        {
            ISQLiteAsyncConnection database = _sqliteConnectionFactory.Create();

            IEnumerable<RoutineDto> routineDtos = await database
                .Table<RoutineDto>()
                .ToListAsync();
            IEnumerable<Guid> routineIds = routineDtos.Select(r => r.Id);

            IEnumerable<RoutineStepDto> routineStepDtos = await database
                .Table<RoutineStepDto>()
                .Where(routineStep => routineIds.Contains(routineStep.RoutineId))
                .ToListAsync();
            ILookup<Guid, RoutineStepDto> routineStepsForRoutine = routineStepDtos
                .ToLookup(r => r.RoutineId);

            return routineDtos.Select(d => new Routine(
                d.Id, 
                d.Name ?? string.Empty, 
                routineStepsForRoutine[d.Id]
                    .OrderBy(s => s.Order)
                    .Select(s => new RoutineStep(
                        s.Name ?? string.Empty, 
                        TimeSpan.FromSeconds(s.DurationSeconds ?? 0)))));
        }
    }
}
