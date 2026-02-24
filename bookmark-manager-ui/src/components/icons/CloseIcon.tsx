export function CloseIcon({ className, onClick }: { className?: string, onClick?:()=>void }){

    return (
        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" className={`fill-neutral-0 stroke-neutral-900 ${className}`} onClick={onClick}>
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="1.6"
                  d="M15 5 5 15M5 5l10 10"/>
        </svg>
    )
}