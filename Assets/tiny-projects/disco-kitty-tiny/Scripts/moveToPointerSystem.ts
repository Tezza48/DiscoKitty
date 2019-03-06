
namespace game {

    /** New System */
    export class moveToPointerSystem extends ut.ComponentSystem {
        
        OnUpdate():void {
            const dt = this.scheduler.deltaTime();
            
            const pointerPosition = ut.Core2D.Input.getInputPosition();
            const worldPointer = ut.Core2D.Input.getWorldInputPosition(this.world);

            this.world.forEach([ut.Entity, game.moveToPointerComponent], (entity, moveToPointer) => {
                this.world.usingComponentData(entity, [ut.Core2D.TransformLocalPosition], (position) => {

                    const direction = worldPointer.sub(position.position);
                    // dont normalize so that it's faster at a distance and slower when close
                    
                    const delta = direction.multiplyScalar(dt * moveToPointer.moveSpeed);
                
                    position.position.add(delta);
                })
            })
        }
    }
}
